
namespace TollEngine
{
    public class Car
    {
        public Car(int passengers, object registration)
        {
            Passengers = passengers;
            Registration = registration;
        }
        public int Passengers { get; set; }
        public object Registration { get; set; }
    }

    public class DeliveryTruck
    {
        public DeliveryTruck(int passengers, int grossWeightClass, object registration)
        {
            Registration = registration;
            GrossWeightClass = grossWeightClass;
        }
        public int GrossWeightClass { get; set; }
        public object Registration { get; set; }
    }

    public class Taxi
    {
        public Taxi(int passengers, object registration)
        {
            Fares = passengers; 
            Registration = registration;
        }
        public int Fares { get; set; }
        public object Registration { get; set; }
    }

    public class Bus
    {
        public Bus(int passengers, int capacity, object registration)
        {
            Riders = passengers;
            Registration = registration;
            Capacity = capacity;
        }
        public int Capacity { get; set; }
        public int Riders { get; set; }
        public object Registration { get; set; }
    }
}
