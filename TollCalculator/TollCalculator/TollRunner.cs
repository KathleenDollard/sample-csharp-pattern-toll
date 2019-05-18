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
                            if (registrationResult.ResultStatus != ResultStatus.Success)
                            {
                                RecordIssue(tollEvent, registrationResult, "Registration");
                            }
                            continue;
                        }
                        IResult<object> vehicleResult = GetVehicle(tollEvent, registrationResult.Data);
                        if (vehicleResult.ResultStatus != ResultStatus.Success)
                        {
                            continue;
                        }
                        var basicToll = tollCalculator.CalculateToll(vehicleResult.Data);
                        var peakPremium = tollCalculator.PeakTimePremium(tollEvent.TollTime, tollEvent.InBound);
                        var toll = basicToll * peakPremium;

                        billingSystem.SendBill(toll, vehicleResult.Data);
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


        private static IResult<object> GetVehicle(TollEvent tollEvent, object registration)
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

        private static void RecordIssue(TollEvent tollEvent, IResult<object> result, string step)
        {
            throw new NotImplementedException();
        }

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
