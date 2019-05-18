using Common;
using System.Linq;

namespace ConsumerVehicleRegistration
{
    public class CarRegistration
    {
        private static readonly CarRegistration[] registeredCars = new CarRegistration[]
        {
            new CarRegistration("71DE148E"),
            new CarRegistration("958F-4D1C"),
            new CarRegistration("32A80A61")
        };

        public CarRegistration(string licensePlate)
            => LicensePlate = licensePlate;

        public string LicensePlate { get; }

        public static Result<CarRegistration> GetByPlate(string licensePlate)
        {
            CarRegistration car = registeredCars.Where(c => c.LicensePlate == licensePlate).SingleOrDefault();
            ResultStatus status = car == null
                                ? ResultStatus.Failure
                                : ResultStatus.Success;
            return new Result<CarRegistration>(status, car);
        }
    }
}

namespace CommercialRegistration
{
    public class DeliveryTruckRegistration
    {

        private static readonly DeliveryTruckRegistration[] registeredCars = new DeliveryTruckRegistration[]
        {

                new DeliveryTruckRegistration("3F2229", 7500),
                new DeliveryTruckRegistration("AC-D26F",4000),
                new DeliveryTruckRegistration("03AC19849",2500)
        };

        public DeliveryTruckRegistration(string licensePlate, int grossWeightClass)
        {
            LicensePlate = licensePlate;
            GrossWeightClass = grossWeightClass;
        }

        public int GrossWeightClass { get; }
        public string LicensePlate { get; }
    }
}

namespace LiveryRegistration
{
    public class TaxiRegistration
    {
        private static readonly TaxiRegistration[] registeredCars = new TaxiRegistration[]
        {
                new TaxiRegistration("3814DB"),
                new TaxiRegistration("3DB9071"),
                new TaxiRegistration("071C7D2E")
        };

        public TaxiRegistration(string licensePlate)
            => LicensePlate = licensePlate;

        public string LicensePlate { get; set; }
    }

    public class BusRegistration
    {
        private static readonly BusRegistration[] registeredCars = new BusRegistration[]
        {

                new BusRegistration("C36745A", 90),
                new BusRegistration("5AAC91A",90),
                new BusRegistration("656D0D",90)
        };

        public BusRegistration(string licensePlate, int capacity)
        {
            LicensePlate = licensePlate;
            Capacity = capacity;
        }

        public int Capacity { get; }
        public string LicensePlate { get; }
    }
}