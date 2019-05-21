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

                IResult<object> vehicleResult = null;
                IResult<object> vehicleRegistrationResult = null;

                foreach (ITollEventSource tollSource in tollEventSources)
                {
                    foreach (TollEvent tollEvent in tollSource.GetTollEvents())
                    {
                        var result = DoOperation(partialFailures, tollEvent,
                                    (prev) => GetVehicleRegistration(tollEvent.LicensePlate),
                                    (prev) => (vehicleRegistrationResult = prev),  // just returns the prev result
                                    (prev) => GetVehicle(prev.Data, tollEvent),
                                    (prev) => (vehicleResult = prev),  // just returns the prev result
                                    (prev) => CalculateToll(prev.Data, tollCalculator, tollEvent),
                                    (prev) => billingSystem.SendBill((decimal)prev.Data, vehicleRegistrationResult.Data));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result<object>.Error(e.Message);
            }
            if (partialFailures.Count > 0)
            {
                return Result<object>.PartialFailure(partialFailures);
            }
            return Result<object>.Success(null);
        }

        private static IResult<object> DoOperation(List<IResult<object>> partialFailures,
                object operationData, params Func<IResult<object>, IResult<object>>[] operations)
        {
            IResult<object> result = null;
            foreach (var operation in operations)
            {
                IResult<object> lastResult = result;
                result = operation(lastResult);
                if (result.ResultStatus != ResultStatus.Success)
                {
                    Logger.LogError($"Step: [TODO: Add Step] - {result.Message}");
                    partialFailures.Add(result);
                    return result;
                }
            }
            return result;
        }

        private static Result<object> CalculateToll(object vehicle,
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
           => registration switch
           {
               ConsumerVehicleRegistration.CarRegistration carReg
                 => Result<object>.Success(new Car(tollEvent.Passengers, carReg)),
               LiveryRegistration.TaxiRegistration taxiReg
                 => Result<object>.Success(new Taxi(tollEvent.Passengers, taxiReg)),
               LiveryRegistration.BusRegistration busReg
                 => Result<object>.Success(new Bus(tollEvent.Passengers,busReg.Capacity, busReg)),
               CommercialRegistration.DeliveryTruckRegistration truckReg
                 => Result<object>.Success(new DeliveryTruck (tollEvent.Passengers, truckReg.GrossWeightClass,truckReg)),
               _ => Result<object>.Failure("Unexpected registration type")
           };

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
