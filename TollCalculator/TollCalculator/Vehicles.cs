using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace TollRunner
{
    public class Car
    {
        public Car(int passengers, CarRegistration registration)
        {
            Passengers = passengers;
            Registration = registration;
        }
        public int Passengers { get; set; }
        public CarRegistration Registration { get; set; }
    }

    public class DeliveryTruck
    {
        public DeliveryTruck(int passengers, DeliveryTruckRegistration registration)
        {
            Registration = registration;
        }
        public int GrossWeightClass { get; set; }
        public DeliveryTruckRegistration Registration { get; set; }
    }

    public class Taxi
    {
        public Taxi(int passengers, TaxiRegistration registration)
        {
            Fares = passengers - 1; //don't count driver
            Registration = registration;
        }
        public int Fares { get; set; }
        public TaxiRegistration Registration { get; set; }
    }

    public class Bus
    {
        public Bus(int passengers, BusRegistration registration)
        {
            Riders = passengers-1;
            Registration = registration;
        }
        public int Capacity { get; set; }
        public int Riders { get; set; }
        public BusRegistration Registration { get; set; }
    }
}
