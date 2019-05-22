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
        private const string RetrieveRegName = "Retrieve registration";
        private const string RetrieveVehicleName = "Retrieve vehicle";
        private const string CalculateTollName = "Calculate toll";
        private const string SendBillName = "Send Bill";

        private static readonly ITollEventSource[] tollEventSources = new ITollEventSource[]
                            {new TollBoothA()};


        public static IResult<object> BillTolls()
        {
            var partialFailures = new List<IResult<object>>();
            var dataBag = new Dictionary<string, object>();
            var result = Handler.Try(() =>
            {
                var tollCalculator = new TollCalculator();
                var billingSystem = new ExternalSystem.BillingSystem();
                var tollEvents = tollEventSources
                    .SelectMany(tollSource => tollSource.GetTollEvents())
                    .Select(tollEvent => BillToll(tollEvent, dataBag, partialFailures, tollCalculator, billingSystem))
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
                                                Dictionary<string, object> dataBag,
                                                List<IResult<object>> partialFailures,
                                                TollCalculator tollCalculator,
                                                ExternalSystem.BillingSystem billingSystem)
            => Handler.Try(()
                => Extensions.DoOperations(partialFailures, dataBag,
                    (RetrieveRegName, (prev) => GetVehicleRegistration(tollEvent.LicencsePlate)),
                    (RetrieveVehicleName, (prev) => GetVehicle(prev.Data, tollEvent)),
                    (CalculateTollName, (prev) => CalculateToll(prev.Data, tollCalculator, tollEvent)),
                    (SendBillName, (prev)
                    => billingSystem.SendBill((decimal)prev.Data, dataBag[RetrieveRegName]))));

        private static IResult<object> CalculateToll(object vehicle,
                TollCalculator tollCalculator, TollEvent tollEvent)
            => Handler.Try(
                () => Result<object>.Success(
                         tollCalculator.CalculateToll(vehicle) *
                         tollCalculator.PeakTimePremium(tollEvent.TollTime, tollEvent.InBound)));

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

        private static IResult<object> GetVehicleRegistration(string licencsePlate)
            => Extensions.DoUntilSuccess<object>(
               () => ConsumerVehicleRegistration.CarRegistration.GetByPlate(licencsePlate),
               () => CommercialRegistration.DeliveryTruckRegistration.GetByPlate(licencsePlate),
               () => LiveryRegistration.TaxiRegistration.GetByPlate(licencsePlate),
               () => LiveryRegistration.BusRegistration.GetByPlate(licencsePlate));

    }
}
