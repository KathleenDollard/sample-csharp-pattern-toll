using Common;
using Common.Find;
using ExternalSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TollEngine;
using TollingData;

namespace TollRunner
{

    public class Runner
    {
        protected static List<Result<object>> partialFailures = new List<Result<object>>();
        protected static Dictionary<string, object> dataBag = new Dictionary<string, object>();
    }

    public class TollRunner : Runner
    {
        private static readonly ITollEventSource[] tollEventSources = new ITollEventSource[]
                            {new TollBoothA()};

        public static Result<object> BillTolls()
            => Handler.Try(() =>
                {
                    var tollEvents = tollEventSources
                        .SelectMany(tollSource => tollSource.GetTollEvents())
                        .Select(tollEvent => BillToll(tollEvent))
                        .ToList();
                    return Result.Success<object>(null);
                });

        private static IResult<object> BillToll(TollEvent tollEvent)
           => tollEvent.Start()
                   .IfNotFailed(GetVehicleRegistration)
                   .IfNotFailed(prev => GetVehicle(prev, tollEvent))
                   .IfNotFailed(prev => CalculateToll(prev, tollEvent))
                   .IfNotFailed(BillingSystem.SendBill);

        private static IResult<object> GetVehicleRegistration(SuccessResult<TollEvent> tollEventResult)
            => tollEventResult.Value.LicensePlate.LookFor<string, object>()
                .IfNotFound(ConsumerVehicleRegistration.CarRegistration.GetByPlate)
                .IfNotFound(CommercialRegistration.DeliveryTruckRegistration.GetByPlate)
                .IfNotFound(LiveryRegistration.TaxiRegistration.GetByPlate)
                .IfNotFound(LiveryRegistration.BusRegistration.GetByPlate)
                .GetValueResult();

        internal static IResult<object> GetVehicle(SuccessResult<object> registrationResult, TollEvent tollEvent)
        {
            // This will switch to a lovely switch expression in C# 8.0
            switch (registrationResult.Value)
            {
                case ConsumerVehicleRegistration.CarRegistration carReg:
                    return Result.Success(new Car(tollEvent.Passengers, carReg));
                case LiveryRegistration.TaxiRegistration taxiReg:
                    return Result.Success(new Taxi(tollEvent.Passengers, taxiReg));
                case LiveryRegistration.BusRegistration busReg:
                    return Result.Success(new Bus(tollEvent.Passengers, busReg.Capacity, busReg));
                case CommercialRegistration.DeliveryTruckRegistration truckReg:
                    return Result.Success(new DeliveryTruck(tollEvent.Passengers,
                        truckReg.GrossWeightClass, truckReg));
                default:
                    return Result.Fail<object>("Unexpected registration type");
            }
        }

        internal static IResult<(decimal, object)> CalculateToll(SuccessResult<object> vehicleResult,
               TollEvent tollEvent)
        {
            // In this case, I think the imperative approach just reads better
            var vehicle = vehicleResult.Value;
            var toll = TollCalculator.CalculateToll(vehicle) *
                       TollCalculator.PeakTimePremium(tollEvent.TollTime, tollEvent.InBound);
            return Result.Success((toll, vehicle));
        }
    }
}
