using TollingData;
using TollEngine;
using Common;
using System;

namespace TollRunner

{
    class Program
    {

        private static readonly ITollEventSource[] tollEventSources
                = new ITollEventSource[]
        {
            new TollBoothA()
        };

        static void Main(string[] args)
        {
            // DisplayTolls();

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
            catch
            {

            }

        }

        private static IResult<object> GetVehicle(TollEvent tollEvent, object registration)
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

            return Result<object>.Failure( "Unexpected registration type");

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