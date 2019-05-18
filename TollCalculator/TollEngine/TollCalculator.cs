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
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            if (vehicle is Car car)
            {
                if (car.Passengers == 0)
                {
                    return carBase + .50m;
                }
                if (car.Passengers == 1)
                {
                    return carBase;
                }
                if (car.Passengers == 2)
                {
                    return carBase - .50m;
                }
                return carBase - 1.0m;
            }

            if (vehicle is Taxi taxi)
            {
                if (taxi.Fares == 0)
                {
                    return taxiBase + 1.00m;
                }
                if (taxi.Fares == 1)
                {
                    return taxiBase;
                }
                if (taxi.Fares == 2)
                {
                    return taxiBase - .50m;
                }
                return taxiBase - 1.0m;
            }

            if (vehicle is Bus bus)
            {
                if ((double)bus.Riders / (double)bus.Capacity < .5)
                {
                    return busBase + 2.00m;
                }
                if ((double)bus.Riders / (double)bus.Capacity > .9)
                {
                    return busBase - 1.00m;
                }
                return busBase;
            }

            if (vehicle is DeliveryTruck truck)
            {
                if ((double)truck.GrossWeightClass > 5000)
                {
                    return truckBase + 5.00m;
                }
                if ((double)truck.GrossWeightClass < 3000)
                {
                    return truckBase - 2.00m;
                }
                return truckBase;
            }
            throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle));
        }

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