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
            var tollCalculator = new TollCalculator();
            var billingSystem = new ExternalSystem.BillingSystem();

            var result = Handler.Try<object>(() =>
            {

                foreach (ITollEventSource tollSource in tollEventSources)
                {
                    foreach (TollEvent tollEvent in tollSource.GetTollEvents())
                    {
                        var operationName = "Retrieve registration";
                        IResult<object> registrationResult = GetVehicleRegistration(tollEvent.LicencsePlate);
                        if (registrationResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(registrationResult, tollEvent, operationName);
                            partialFailures.Add(registrationResult);
                            return registrationResult;
                        }
                        Console.WriteLine($"{operationName} complete");

                        operationName = "Retrieve vehicle";
                        IResult<object> vehicleResult = GetVehicle(registrationResult.Data, tollEvent);
                        if (vehicleResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(vehicleResult, tollEvent, operationName);
                            partialFailures.Add(vehicleResult);
                            return vehicleResult;
                        }
                        Console.WriteLine($"{operationName} complete");

                        operationName = "Calculate toll";
                        IResult<decimal> tollResult = CalculateToll(vehicleResult.Data, tollCalculator, tollEvent);
                        if (tollResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue((IResult<object>)tollResult, tollEvent, operationName);
                            partialFailures.Add((IResult<object>)tollResult);
                            return (IResult<object>)tollResult;
                        }
                        Console.WriteLine($"{operationName} complete");

                        operationName = "Send Bill";
                        IResult<object> billResult = billingSystem.SendBill(tollResult.Data, registrationResult.Data);
                        if (billResult.ResultStatus != ResultStatus.Success)
                        {
                            RecordIssue(billResult, tollEvent, operationName);
                            partialFailures.Add(billResult);
                            return billResult;
                        }
                        Console.WriteLine($"{operationName} complete");
                    }
                }
                return Result<object>.Success(null);
            });
            if (partialFailures.Count > 0)
            {
                return Result<object>.PartialFailure(partialFailures);
            }

            return result;
        }

        public IResult<T> DoOperation<T>(List<IResult<T>> partialFailures, string operationName,
            object operationData, Func<IResult<T>> operation)
        {
            IResult<T> result = operation();
            if (result.ResultStatus != ResultStatus.Success)
            {
                RecordIssue((IResult<object>)result, operationData, operationName);
                partialFailures.Add(result);
                return result;
            }
            Console.WriteLine($"{operationName} complete");
            return result;
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
        {
            if (registration is ConsumerVehicleRegistration.CarRegistration carReg)
            {
                return Result<object>.Success(new Car(tollEvent.Passengers, carReg));
            }

            if (registration is LiveryRegistration.TaxiRegistration taxiReg)
            {
                return Result<object>.Success(new Taxi(tollEvent.Passengers, taxiReg));
            }

            if (registration is LiveryRegistration.BusRegistration busReg)
            {
                return Result<object>.Success(new Bus(tollEvent.Passengers, busReg.Capacity, busReg));
            }

            if (registration is CommercialRegistration.DeliveryTruckRegistration truckReg)
            {
                return Result<object>.Success(new DeliveryTruck(tollEvent.Passengers, truckReg.GrossWeightClass, truckReg));
            }

            return Result<object>.Failure("Unexpected registration type");

        }

        private static void RecordIssue(IResult<object> result, object operationData, string step)
            => Logger.Log(result.Message, Severity.Error);

        private static IResult<object> GetVehicleRegistration(string licensePlate) 
            => TryGetRegistration(licensePlate,
                ConsumerVehicleRegistration.CarRegistration.GetByPlate,
                CommercialRegistration.DeliveryTruckRegistration.GetByPlate,
                LiveryRegistration.TaxiRegistration.GetByPlate,
                LiveryRegistration.BusRegistration.GetByPlate
                );

        private static IResult<object>  TryGetRegistration(string licensePlate,
                params Func<string,IResult<object>>[] tryOperations)
        {
            foreach (var tryOperation in tryOperations )
            {
                var result = tryOperation(licensePlate);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    return result;
                }
            }
            return Result<object>.Failure("Could not find registration");
        }

    }
}
