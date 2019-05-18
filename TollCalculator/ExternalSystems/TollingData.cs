using System;
using System.Collections.Generic;

namespace TollingData
{
    public class TollEvent
    {
        public TollEvent(DateTime tollTime, object vehicle)
        {
            TollTime = tollTime;
            Vehicle = vehicle;
        }
        DateTime TollTime { get; }
        object Vehicle { get; }
    }

    public class TollBoothA
    {
        private IEnumerable<string, DateTime> GetTollEvents()
            =>new  Dictionary<string, DateTime>
            {
            "71DE148E"
            };
    }
}

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