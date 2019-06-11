using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using NUnit.Framework;
using TollEngine;

namespace Tests
{
    public class TollCalculatorTests
    {
        [Test]
        public void Car_with_no_passengers_correct()
        {
            var carRegistration = new CarRegistration("Test");
            var car = new Car(0, carRegistration);
            var toll = TollCalculator.CalculateToll(car);
            Assert.AreEqual(2.50m, toll);
        }

        [Test]
        public void Car_with_one_passengers_correct()
        {
            var carRegistration = new CarRegistration("Test");
            var car = new Car(1, carRegistration);
            var toll = TollCalculator.CalculateToll(car);
            Assert.AreEqual(2.00m, toll);
        }

        [Test]
        public void Car_with_two_passengers_correct()
        {
            var carRegistration = new CarRegistration("Test");
            var car = new Car(2, carRegistration);
            var toll = TollCalculator.CalculateToll(car);
            Assert.AreEqual(1.50m, toll);
        }

        [Test]
        public void Car_with_five_passengers_correct()
        {
            var carRegistration = new CarRegistration("Test");
            var car = new Car(5, carRegistration);
            var toll = TollCalculator.CalculateToll(car);
            Assert.AreEqual(1.00m, toll);
        }

        [Test]
        public void Car_with_ten_passengers_correct()
        {
            var carRegistration = new CarRegistration("Test");
            var car = new Car(10, carRegistration);
            var toll = TollCalculator.CalculateToll(car);
            Assert.AreEqual(1.0m, toll);
        }

        [Test]
        public void Taxi_with_no_passengers_correct()
        {
            var taxiRegistration = new TaxiRegistration("Test");
            var taxi = new Taxi(0, taxiRegistration);
            var toll = TollCalculator.CalculateToll(taxi);
            Assert.AreEqual(4.50m, toll);
        }

        [Test]
        public void Taxi_with_one_passengers_correct()
        {
            var taxiRegistration = new TaxiRegistration("Test");
            var taxi = new Taxi(1, taxiRegistration);
            var toll = TollCalculator.CalculateToll(taxi);
            Assert.AreEqual(3.50m, toll);
        }

        [Test]
        public void Taxi_with_two_passengers_correct()
        {
            var taxiRegistration = new TaxiRegistration("Test");
            var taxi = new Taxi(2, taxiRegistration);
            var toll = TollCalculator.CalculateToll(taxi);
            Assert.AreEqual(3.00m, toll);
        }

        [Test]
        public void Taxi_with_five_passengers_correct()
        {
            var taxiRegistration = new TaxiRegistration("Test");
            var taxi = new Taxi(5, taxiRegistration);
            var toll = TollCalculator.CalculateToll(taxi);
            Assert.AreEqual(2.50m, toll);
        }

        [Test]
        public void Taxi_with_ten_passengers_correct()
        {
            var taxiRegistration = new TaxiRegistration("Test");
            var taxi = new Taxi(10, taxiRegistration);
            var toll = TollCalculator.CalculateToll(taxi);
            Assert.AreEqual(2.50m, toll);
        }

        [Test]
        public void Bus_with_low_occupancy_correct()
        {
            var busRegistration = new BusRegistration("Test", 90);
            var bus = new Bus(15, busRegistration.Capacity,busRegistration);
            var toll = TollCalculator.CalculateToll(bus);
            Assert.AreEqual(7.00m, toll);
        }

        [Test]
        public void Bus_with_normal_occupancy_correct()
        {
            var busRegistration = new BusRegistration("Test", 90);
            var bus = new Bus(75, busRegistration.Capacity, busRegistration);
            var toll = TollCalculator.CalculateToll(bus);
            Assert.AreEqual(5.00m, toll);
        }

        [Test]
        public void Bus_with_high_occupancy_correct()
        {
            var busRegistration = new BusRegistration("Test", 90);
            var bus = new Bus(85, busRegistration.Capacity, busRegistration);
            var toll = TollCalculator.CalculateToll(bus);
            Assert.AreEqual(4.00m, toll);
        }

        [Test]
        public void Truck_that_is_light_correct()
        {
            var lightTruckReg = new DeliveryTruckRegistration("Test", 2500);
            var truck = new DeliveryTruck(0, lightTruckReg.GrossWeightClass , lightTruckReg);
            var toll = TollCalculator.CalculateToll(truck);
            Assert.AreEqual(8.00m, toll);
        }


        [Test]
        public void Truck_that_is_normal_correct()
        {
            var truckReg = new DeliveryTruckRegistration("Test", 4000);
            var truck = new DeliveryTruck(0, truckReg.GrossWeightClass, truckReg);
            var toll = TollCalculator.CalculateToll(truck);
            Assert.AreEqual(10.00m, toll);
        }


        [Test]
        public void Truck_that_is_heavy_correct()
        {
            var heavyTruckReg = new DeliveryTruckRegistration("Test", 7500);
            var truck = new DeliveryTruck(0, heavyTruckReg.GrossWeightClass, heavyTruckReg); ;
            var toll = TollCalculator.CalculateToll(truck);
            Assert.AreEqual(15.00m, toll);
        }


    }
}