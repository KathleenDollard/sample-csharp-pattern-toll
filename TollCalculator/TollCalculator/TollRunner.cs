using Common;
using System;
using System.Collections.Generic;
using System.Text;
using TollEngine;
using TollingData;
using System.Linq;

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
                foreach (var tollEventSource in tollEventSources)
                {
                    foreach (var tollEvent in tollEventSource.GetTollEvents())
                    {
                        BillToll(tollEvent, partialFailures, tollCalculator, billingSystem);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Result<object>.Exception(e.Message);
            }
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
                var operationName = "Retrieve registration";
                var registrationResult = GetVehicleRegistration(tollEvent.LicencsePlate);
                if (registrationResult.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue(registrationResult, tollEvent, operationName);
                    partialFailures.Add(registrationResult);
                    return registrationResult;
                }
                Console.WriteLine($"{operationName} complete");

                operationName = "Retrieve vehicle";
                var vehicleResult = GetVehicle(registrationResult.Data, tollEvent);
                if (vehicleResult.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue(vehicleResult, tollEvent, operationName);
                    partialFailures.Add(vehicleResult);
                    return vehicleResult;
                }
                Console.WriteLine($"{operationName} complete");

                operationName = "Calculate toll";
                var tollResult = CalculateToll(vehicleResult.Data, tollCalculator, tollEvent);
                if (tollResult.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue((IResult<object>)tollResult, tollEvent, operationName);
                    partialFailures.Add((IResult<object>)tollResult);
                    return tollResult;
                }
                Console.WriteLine($"{operationName} complete");

                operationName = "Send Bill";
                var billResult = billingSystem.SendBill((decimal)tollResult.Data, registrationResult.Data);
                if (billResult.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue(billResult, tollEvent, operationName);
                    partialFailures.Add(billResult);
                    return billResult;
                }
                Console.WriteLine($"{operationName} complete");
                return billResult;
            }
            catch
            {
                return Result<object>.PartialFailure(partialFailures);
            }
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
            var carReg = registration as ConsumerVehicleRegistration.CarRegistration;
            if (carReg != null)
            {
                return Result<object>.Success(new Car(tollEvent.Passengers, carReg));
            }

            var taxiReg = registration as LiveryRegistration.TaxiRegistration;
            if (taxiReg != null)
            {
                return Result<object>.Success(new Taxi(tollEvent.Passengers, taxiReg));
            }

            var busReg = registration as LiveryRegistration.BusRegistration;
            if (busReg != null)
            {
                return Result<object>.Success(new Bus(tollEvent.Passengers, busReg.Capacity, busReg));
            }

            var truckReg = registration as CommercialRegistration.DeliveryTruckRegistration;
            if (truckReg != null)
            {
                return Result<object>.Success(new DeliveryTruck(tollEvent.Passengers, truckReg.GrossWeightClass, truckReg));
            }

            return Result<object>.Failure("Unexpected registration type");

        }

        private static void RecordIssue(IResult<object> result, TollEvent tollEvent, string step)
          => Logger.Log(result.Message, Severity.Error);

        private static IResult<object> GetVehicleRegistration(string licencsePlate)
        {
            IResult<object> vehicleResult = ConsumerVehicleRegistration.CarRegistration.GetByPlate(licencsePlate);
            if (vehicleResult.ResultStatus != ResultStatus.Success)
            {
                vehicleResult = CommercialRegistration.DeliveryTruckRegistration.GetByPlate(licencsePlate);
            }
            if (vehicleResult.ResultStatus != ResultStatus.Success)
            {
                vehicleResult = LiveryRegistration.TaxiRegistration.GetByPlate(licencsePlate);
            }
            if (vehicleResult.ResultStatus != ResultStatus.Success)
            {
                vehicleResult = LiveryRegistration.BusRegistration.GetByPlate(licencsePlate);
            }
            return vehicleResult;
        }

    }
}
