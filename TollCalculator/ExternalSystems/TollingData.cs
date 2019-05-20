using System;
using System.Collections.Generic;

namespace TollingData
{
    public class TollEvent
    {
        public TollEvent(int passengers, DateTime tollTime, bool inBound, string licensePlate)
        {
            TollTime = tollTime;
            InBound = inBound;
            LicensePlate = licensePlate;
            Passengers = passengers;
        }
        public DateTime TollTime { get; }
        public bool InBound { get; }
        public string LicensePlate { get; }
        public int Passengers { get; }
    }

    public interface ITollEventSource
    {
        IEnumerable<TollEvent> GetTollEvents();
    }


    public class TollBoothA : ITollEventSource
    {
        public IEnumerable<TollEvent> GetTollEvents()
        => new TollEvent[]
        {
            // DayTime
            new TollEvent(0, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 6, 11, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 6, 11, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 6, 11, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019, 3, 6, 11, 30, 0), true,licensePlate: "03AC19849"),
                                                              
            //EveningRush                                     
            new TollEvent(0, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 7, 17, 15, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 7, 17, 15, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 7, 17, 15, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019,  3, 7, 17, 15, 0),true, licensePlate: "03AC19849"),
                                                              
            // Overnight                                      
            new TollEvent(0, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 14, 03, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 14, 03, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 14, 03, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019,  3, 14, 03, 30, 0),true, licensePlate: "03AC19849"),
                                                               
            // Weekend morning rush                            
            new TollEvent(0, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 16, 8, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 16, 8, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 16, 8, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019,  3, 16, 8, 30, 0),true, licensePlate: "03AC19849"),
                                                                 
            // Weekend daytime                                   
            new TollEvent(0, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 17, 14, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 17, 14, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 17, 14, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019,  3, 17, 14, 30, 0),true, licensePlate: "03AC19849"),
                                                                
            // Weekend Evening rush                             
            new TollEvent(0, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019,  3, 17, 18, 05, 0),true,licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019, 3, 17, 18, 05, 0),true,licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019, 3, 17, 18, 05, 0),true,licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019, 3, 17, 18, 05, 0),true,licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019,  3, 17, 18, 05, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019,  3, 17, 18, 05, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019,  3, 17, 18, 05, 0),true, licensePlate: "03AC19849"),
                                                                
            // Weekend overnight                                
            new TollEvent(0, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(1, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(2, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(5, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(9, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "71DE148E"),
            new TollEvent(0, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(1, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(2, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(5, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(9, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3DB9071"),
            new TollEvent(15, new DateTime(2019,3, 16, 01, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(75, new DateTime(2019,3, 16, 01, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(85, new DateTime(2019,3, 16, 01, 30, 0),true, licensePlate: "656D0D"),
            new TollEvent(0, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "3F2229"),
            new TollEvent(1, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "AC-D26F"),
            new TollEvent(2, new DateTime(2019, 3, 16, 01, 30, 0),true, licensePlate: "03AC19849"),
};
    }
}

//var testTimes = new DateTime[]

//    {
//            new DateTime(2019, 3, 6, 11, 30, 0), // daytime
//            new DateTime(2019, 3, 7, 17, 15, 0), // evening rush
//            new DateTime(2019, 3, 14, 03, 30, 0), // overnight

//            new DateTime(2019, 3, 16, 8, 30, 0), // weekend morning rush
//            new DateTime(2019, 3, 17, 14, 30, 0), // weekend daytime
//            new DateTime(2019, 3, 17, 18, 05, 0), // weekend evening rush
//            new DateTime(2019, 3, 16, 01, 30, 0), // weekend overnight
//    };



//new Car(),
//            new Car { Passengers = 1 },
//            new Car { Passengers = 2 },
//            new Car { Passengers = 5 },
//            new Taxi(),
//            new Taxi { Fares = 1 },
//            new Taxi { Fares = 2 },
//            new Taxi { Fares = 5 },
//            new Bus { Capacity = 90, Riders = 15 },
//            new Bus { Capacity = 90, Riders = 75 },
//            new Bus { Capacity = 90, Riders = 85 },

//            new DeliveryTruck { GrossWeightClass = 7500 },
//            new DeliveryTruck { GrossWeightClass = 4000 },
//            new DeliveryTruck { GrossWeightClass = 2500 },