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

    public class Runner
    {
        protected static List<IResult<object>> partialFailures = new List<IResult<object>>();
        protected static Dictionary<string, object> dataBag = new Dictionary<string, object>();
    }

    public class TollRunner : Runner
    {
        private const string RetrieveRegName = "Retrieve registration";
        private const string RetrieveVehicleName = "Retrieve vehicle";
        private const string CalculateTollName = "Calculate toll";
        private const string SendBillName = "Send Bill";

        private static readonly TollCalculator tollCalculator = new TollCalculator();
        private static readonly ExternalSystem.BillingSystem billingSystem = new ExternalSystem.BillingSystem();

        private static readonly ITollEventSource[] tollEventSources = new ITollEventSource[]
                            {new TollBoothA()};

        public static IResult<object> BillTolls()
            => Handler.Try(() =>
                {
                    var tollEvents = tollEventSources
                        .SelectMany(tollSource => tollSource.GetTollEvents())
                        .Select(tollEvent => BillToll(tollEvent))
                        .ToList();
                    return Result<object>.Success(null);
                });

        private static IResult<object> BillToll(TollEvent tollEvent)
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
