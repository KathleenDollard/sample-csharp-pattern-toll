using System;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace toll_calculator
{
    public partial class TollCalculator
    {
        public decimal CalculateToll(object vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }

            var car = vehicle as Car;
            if (car != null)
            {
                if (car.Passengers == 0)
                {
                    return 2.00m + .50m;
                }
                if (car.Passengers == 1)
                {
                    return 2.00m;
                }
                if (car.Passengers == 2)
                {
                    return 2.00m - .50m;
                }
                return 2.00m - 1.0m;
            }

            var taxi = vehicle as Taxi;
            if (taxi != null)
            {
                if (taxi.Fares == 0)
                {
                    return 3.50m + 1.00m;
                }
                if (taxi.Fares == 1)
                {
                    return 3.50m;
                }
                if (taxi.Fares == 2)
                {
                    return 3.50m - .50m;
                }
                return 3.50m - 1.0m;
            }

            var bus = vehicle as Bus;
            if (bus != null)
            {
                if ((double)bus.Riders / (double)bus.Capacity < .5)
                {
                    return 5.00m + 2.00m;
                }
                if ((double)bus.Riders / (double)bus.Capacity > .9)
                {
                    return 5.00m - 1.00m;
                }
                return 5.00m;
            }

            var truck = vehicle as DeliveryTruck;
            if (truck != null)
            {
                if ((double)truck.GrossWeightClass > 5000)
                {
                    return 10.00m + 5.00m;
                }
                if ((double)truck.GrossWeightClass < 3000)
                {
                    return 10.00m - 2.00m;
                }
                return 10.00m;
            }
            throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle));
        }

        public decimal PeakTimePremium(DateTime timeOfToll, bool inbound)
        {
            if (Extensions.IsWeekDay(timeOfToll))
            {

                switch (GetTimeBand(timeOfToll))
                {
                    case TimeBand.Overnight:
                        return 0.75m;
                    case TimeBand.Daytime:
                        return 1.50m;
                    case TimeBand.MorningRush:
                        return inbound
                                ? 2.00m
                                : 1.00m;
                    case TimeBand.EveningRush:
                        return !inbound
                                ? 2.00m
                                : 1.00m;
                        throw new ArgumentException(message: "Not a known time band", paramName: timeOfToll.ToString());
                }
            }
            return 1.00m;
        }

        private static TimeBand GetTimeBand(DateTime timeOfToll)
        {
            var hour = timeOfToll.Hour;
            if (hour < 6)
            {
                return TimeBand.Overnight;
            }
            else if (hour < 10)
            {
                return TimeBand.MorningRush;
            }
            else if (hour < 16)
            {
                return TimeBand.Daytime;
            }
            else if (hour < 20)
            {
                return TimeBand.EveningRush;
            }
            else
            {
                return TimeBand.Overnight;
            }
        }
    }
}