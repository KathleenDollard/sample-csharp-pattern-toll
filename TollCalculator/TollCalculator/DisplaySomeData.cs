using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;
using TollEngine;

namespace TollRunner
{
    public class DisplaySomeData
    { 
        internal static void DisplayTolls()
        {
            var carRegistration = new CarRegistration("Test");
            var soloDriver = new Car(0,carRegistration);
            var twoRideShare = new Car(1,carRegistration);
            var threeRideShare = new Car(2,carRegistration);
            var fullVan = new Car(5, carRegistration);

            var taxiRegistration = new TaxiRegistration("Test");
            var emptyTaxi = new Taxi(0, taxiRegistration);
            var singleFare = new Taxi(1, taxiRegistration);
            var doubleFare = new Taxi(2, taxiRegistration);
            var fullVanPool = new Taxi(5, taxiRegistration);

            var busRegistration = new BusRegistration("Test",90);
            var lowOccupantBus = new Bus(15, busRegistration.Capacity ,busRegistration);
            var normalBus = new Bus(75, busRegistration.Capacity, busRegistration);
            var fullBus = new Bus(85, busRegistration.Capacity, busRegistration);

            var lightTruckReg = new DeliveryTruckRegistration("Test", 7500);
            var truckReg = new DeliveryTruckRegistration("Test", 4000);
            var heavyTruckReg = new DeliveryTruckRegistration("Test", 2500);
            var heavyTruck = new DeliveryTruck (0, heavyTruckReg.GrossWeightClass ,heavyTruckReg);
            var truck = new DeliveryTruck(0, truckReg.GrossWeightClass, truckReg);
            var lightTruck = new DeliveryTruck(0, lightTruckReg.GrossWeightClass, lightTruckReg);

            Console.WriteLine($"The toll for a solo driver is {TollCalculator.CalculateToll(soloDriver)}");
            Console.WriteLine($"The toll for a two ride share is {TollCalculator.CalculateToll(twoRideShare)}");
            Console.WriteLine($"The toll for a three ride share is {TollCalculator.CalculateToll(threeRideShare)}");
            Console.WriteLine($"The toll for a fullVan is {TollCalculator.CalculateToll(fullVan)}");

            Console.WriteLine($"The toll for an empty taxi is {TollCalculator.CalculateToll(emptyTaxi)}");
            Console.WriteLine($"The toll for a single fare taxi is {TollCalculator.CalculateToll(singleFare)}");
            Console.WriteLine($"The toll for a double fare taxi is {TollCalculator.CalculateToll(doubleFare)}");
            Console.WriteLine($"The toll for a full van taxi is {TollCalculator.CalculateToll(fullVanPool)}");

            Console.WriteLine($"The toll for a low-occupant bus is {TollCalculator.CalculateToll(lowOccupantBus)}");
            Console.WriteLine($"The toll for a regular bus is {TollCalculator.CalculateToll(normalBus)}");
            Console.WriteLine($"The toll for a bus is {TollCalculator.CalculateToll(fullBus)}");

            Console.WriteLine($"The toll for a heavy truck is {TollCalculator.CalculateToll(heavyTruck)}");
            Console.WriteLine($"The toll for a truck is {TollCalculator.CalculateToll(truck)}");
            Console.WriteLine($"The toll for a light truck is {TollCalculator.CalculateToll(lightTruck)}");

            try
            {
                TollCalculator.CalculateToll("this will fail");
            }
            catch (ArgumentException )
            {
                Console.WriteLine("Caught an argument exception when using the wrong type");
            }
            try
            {
                TollCalculator.CalculateToll(null);
            }
            catch (ArgumentNullException )
            {
                Console.WriteLine("Caught an argument exception when using null");
            }
            Console.WriteLine("Testing the time premiums");

            var testTimes = new DateTime[]
             {
                new DateTime(2019, 3, 4, 8, 0, 0), // morning rush
                new DateTime(2019, 3, 6, 11, 30, 0), // daytime
                new DateTime(2019, 3, 7, 17, 15, 0), // evening rush
                new DateTime(2019, 3, 14, 03, 30, 0), // overnight

                new DateTime(2019, 3, 16, 8, 30, 0), // weekend morning rush
                new DateTime(2019, 3, 17, 14, 30, 0), // weekend daytime
                new DateTime(2019, 3, 17, 18, 05, 0), // weekend evening rush
                new DateTime(2019, 3, 16, 01, 30, 0), // weekend overnight
             };

            Console.WriteLine("====================================================");
            foreach (DateTime time in testTimes)
            {
                Console.WriteLine($"Inbound premium at {time} is {TollCalculator.PeakTimePremium(time, true)}");
                Console.WriteLine($"Outbound premium at {time} is {TollCalculator.PeakTimePremium(time, false)}");
            }
        }
    }
}