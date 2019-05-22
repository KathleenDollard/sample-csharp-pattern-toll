using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TollEngine;
using TollingData;

namespace TollRunner
{
    public class TollRunner
    {
        private static readonly ITollEventSource[] tollEventSources
          = new ITollEventSource[]
        {
                new TollBoothA()
        };


        public static IResult<object> BillTolls()
        {
            var partialFailures = new List<IResult<object>>();
            var result = Handler.Try(() =>
            {
                var tollCalculator = new TollCalculator();
                var billingSystem = new ExternalSystem.BillingSystem();
                var tollEvents = tollEventSources
                    .SelectMany(tollSource => tollSource.GetTollEvents())
                    .Select(tollEvent => BillToll(tollEvent, partialFailures, tollCalculator, billingSystem))
                    .ToList();
                return Result<object>.Success(null);
            });
            if (partialFailures.Count > 0)
            {
                Logger.LogError("Partial Failure");
                return Result<object>.PartialFailure(partialFailures);
            }
            return Result<object>.Success(null);
        }


        private static IResult<object> BillToll(TollEvent tollEvent,
                                                List<IResult<object>> partialFailures,
                                                TollCalculator tollCalculator,
                                                ExternalSystem.BillingSystem billingSystem)
        {
            try
            {
                IResult<object> registrationResult = null;
                return DoOperations(partialFailures,
                    ("Retrieve registration", (result) => GetVehicleRegistration(tollEvent.LicencsePlate)),
                    ("Assign registration to temp", (Result)=> registrationResult = Result),
                    ("Retrieve vehicle", (result) => GetVehicle(result.Data, tollEvent)),
                    ("Calculate toll", (result) => CalculateToll(result.Data, tollCalculator, tollEvent)),
                    ("Send Bill", (result) => billingSystem.SendBill((decimal)result.Data, registrationResult.Data)));
            }
            catch
            {
                return Result<object>.PartialFailure(partialFailures);
            }
        }

        public static IResult<object> DoOperations(List<IResult<object>> partialFailures,
                params (string , Func<IResult<object>, IResult<object>>)[] operationTuples)
        {
            IResult<object> result = null;
            foreach (var (operationName, operation) in operationTuples)
            {
                result = operation(result);
                if (result.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue(result, operationName);
                    partialFailures.Add(result);
                    return result;
                }
                Console.WriteLine($"{operationName} complete");
            }
            return result;
        }

        private static IResult<object> CalculateToll(object vehicle,
         TollCalculator tollCalculator, TollEvent tollEvent)
        {
            try
            {
                var basicToll = tollCalculator.CalculateToll(vehicle);
                var peakPremium = tollCalculator.PeakTimePremium(tollEvent.TollTime, tollEvent.InBound);
                var toll = basicToll * peakPremium;
                return Result<object>.Success(toll);
            }
            catch (Exception e)
            {
                return Result<object>.Error(e.Message);
            }
        }


        private static IResult<object> GetVehicle(object registration, TollEvent tollEvent)
        {
            switch (registration)
            {
                case ConsumerVehicleRegistration.CarRegistration carReg:
                    return Result<object>.Success(new Car(tollEvent.Passengers, carReg));
                case LiveryRegistration.TaxiRegistration taxiReg:
                    return Result<object>.Success(new Taxi(tollEvent.Passengers, taxiReg));
                case LiveryRegistration.BusRegistration busReg:
                    return Result<object>.Success(new Bus(tollEvent.Passengers, busReg.Capacity, busReg));
                case CommercialRegistration.DeliveryTruckRegistration truckReg:
                    return Result<object>.Success(new DeliveryTruck(tollEvent.Passengers,
                        truckReg.GrossWeightClass, truckReg));
            }
            return Result<object>.Failure("Unexpected registration type");
        }

        private static void RecordIssue(IResult<object> result, string step)
          => Logger.Log(result.Message, Severity.Error);

        private static IResult<object> GetVehicleRegistration(string licencsePlate)
            => Extensions.DoUntilSuccess<object>(
               () => ConsumerVehicleRegistration.CarRegistration.GetByPlate(licencsePlate),
               () => CommercialRegistration.DeliveryTruckRegistration.GetByPlate(licencsePlate),
               () => LiveryRegistration.TaxiRegistration.GetByPlate(licencsePlate),
               () => LiveryRegistration.BusRegistration.GetByPlate(licencsePlate));

    }
}
