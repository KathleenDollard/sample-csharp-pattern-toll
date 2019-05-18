using System;
using Common;

namespace TollEngine
{
    public partial class TollCalculator
    {
        private const decimal carBase = 2.00m;
        private const decimal taxiBase = 3.50m;
        private const decimal busBase = 5.00m;
        private const decimal truckBase = 10.00m;

        private const decimal peakPremiumBase = 1.00m;
        private const int morningRushStart = 6;
        private const int morningRushEnd = 10;
        private const int eveningRushStart = 12 + 4;  // helping folks not skilled with 24 hour clocks
        private const int eveningRushEnd = 12 + 8;

        public decimal CalculateToll(object vehicle)
             => vehicle switch
             {
                 null => throw new ArgumentNullException(nameof(vehicle)),
                 Car { Passengers: 0 } => carBase + .50m,
                 Car { Passengers: 1 } => carBase,
                 Car { Passengers: 2 } => carBase - .50m,
                 Car _ => carBase - 1.00m,

                 Taxi { Fares: 0 } => taxiBase + 1.00m,
                 Taxi { Fares: 1 } => taxiBase,
                 Taxi { Fares: 2 } => taxiBase - .50m,
                 Taxi _ => taxiBase - 1.00m,

                 Bus b when ((double)b.Riders / (double)b.Capacity < .5) => busBase + 2.00m,
                 Bus b when ((double)b.Riders / (double)b.Capacity > .9) => busBase - 1.00m,
                 Bus _ => busBase,

                 DeliveryTruck t when ((double)t.GrossWeightClass > 5000) => truckBase + 5.00m,
                 DeliveryTruck t when ((double)t.GrossWeightClass < 3000) => truckBase - 2.00m,
                 DeliveryTruck _ => truckBase,

                 _ => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle))
             };

            public decimal PeakTimePremium(DateTime timeOfToll, bool inbound)
        {
            TimeBand timeBand = GetTimeBand(timeOfToll);
            if (Extensions.IsWeekDay(timeOfToll))
            {

                switch (timeBand)
                {
                    case TimeBand.Overnight:
                        return 0.75m;
                    case TimeBand.Daytime:
                        return 1.50m;
                    case TimeBand.MorningRush:
                        return inbound
                                ? 2.00m
                                : peakPremiumBase;
                    case TimeBand.EveningRush:
                        return !inbound
                                ? 2.00m
                                : peakPremiumBase;
                        throw new ArgumentException(message: "Not a known time band", paramName: timeOfToll.ToString());
                }
            }
            return timeBand == TimeBand.Overnight
                                ? 0.75m
                                : peakPremiumBase;
        }

        private static TimeBand GetTimeBand(DateTime timeOfToll)
            => GetTimeBandFromHour(timeOfToll.Hour);

        private static TimeBand GetTimeBandFromHour(int hour) 
            => hour < morningRushStart
                    ? TimeBand.Overnight
                    : hour < morningRushEnd
                        ? TimeBand.MorningRush
                        : hour < eveningRushStart
                            ? TimeBand.Daytime
                            : hour < eveningRushEnd
                                ? TimeBand.EveningRush
                                : TimeBand.Overnight;
    }
}