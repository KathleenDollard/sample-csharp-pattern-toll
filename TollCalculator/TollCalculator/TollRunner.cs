using Common;
using System;
using System.Collections.Generic;
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
            try
            {
                var tollCalculator = new TollCalculator();
                var billingSystem = new ExternalSystem.BillingSystem();

                foreach (ITollEventSource tollSource in tollEventSources)
                {
                    foreach (TollEvent tollEvent in tollSource.GetTollEvents())
                    {
                        IResult<object> registrationResult = GetVehicleRegistration(tollEvent.LicencsePlate);
                        if (registrationResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(registrationResult, tollEvent, "Retrieve registration");
                            partialFailures.Add(registrationResult);
                            continue;
                        }
                        IResult<object> vehicleResult = GetVehicle(registrationResult.Data, tollEvent);
                        if (vehicleResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(registrationResult, tollEvent, "Get vehicle");
                            partialFailures.Add(registrationResult);
                            continue;
                        }
                        IResult<decimal> tollResult = CalculateToll(vehicleResult.Data, tollCalculator, tollEvent);
                        if (vehicleResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(registrationResult, tollEvent, "Calculate toll");
                            partialFailures.Add(vehicleResult);
                            continue;
                        }
                        IResult<object> billResult = billingSystem.SendBill(tollResult.Data, vehicleResult.Data);
                        if (billResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(billResult, tollEvent, "Calculate toll");
                            partialFailures.Add(registrationResult);
                            continue;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Result<object>.Error(e.Message);
            }
            if (partialFailures.Count > 0)
            {
                return Result<object>.PartialFailure(partialFailures);
            }
            return Result<object>.Success(null);
        }

        private static Result<decimal> CalculateToll(object vehicle,
                TollCalculator tollCalculator, TollEvent tollEvent)
        {
            try
            {
                var basicToll = tollCalculator.CalculateToll(vehicle);
                var peakPremium = tollCalculator.PeakTimePremium(tollEvent.TollTime, tollEvent.InBound);
                var toll = basicToll * peakPremium;
                return Result<decimal>.Success(toll);
            }
            catch (Exception e)
            {
                return Result<decimal>.Error(e.Message);
            }
        }

        private static IResult<object> GetVehicle(object registration, TollEvent tollEvent)
           => registration switch
           {
              ConsumerVehicleRegistration.CarRegistration carReg 
                    => Result<object>.Success(new Car(tollEvent.Passengers, carReg)),
              LiveryRegistration.TaxiRegistration carReg 
                    => Result<object>.Success(new Car(tollEvent.Passengers, carReg)),
              LiveryRegistration.BusRegistration carReg 
                    => Result<object>.Success(new Car(tollEvent.Passengers, carReg)),
              CommercialRegistration.DeliveryTruckRegistration carReg  
                    => Result<object>.Success(new Car(tollEvent.Passengers, carReg)),
              _ => Result<object>.Failure("Unexpected registration type")
           };

        private static void RecordIssue(IResult<object> result, TollEvent tollEvent, string step)
        {
            throw new NotImplementedException();
        }

        private static IResult<object> GetVehicleRegistration(string licensePlate)
            => GetRegistrationFromSource(licensePlate,
                    ConsumerVehicleRegistration.CarRegistration.GetByPlate,
                    CommercialRegistration.DeliveryTruckRegistration.GetByPlate,
                    LiveryRegistration.TaxiRegistration.GetByPlate,
                    LiveryRegistration.BusRegistration.GetByPlate);


        public static IResult<object> GetRegistrationFromSource(string licensePlate,
                params Func<string, IResult<object>>[] sourceLookups)
        {
            IResult<object> vehicleResult = null;
            foreach (var sourceLookup in sourceLookups)
            {
                vehicleResult = sourceLookup(licensePlate);
                if (vehicleResult.ResultStatus == ResultStatus.Success)
                {
                    return vehicleResult;
                }
            }
            return vehicleResult;
        }

    }
}
