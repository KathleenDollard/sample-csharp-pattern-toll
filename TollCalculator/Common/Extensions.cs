using System;

namespace Common
{
    public class Extensions
    {
        // Not an extension to avoid confusion with example
        public static bool IsWeekDay(DateTime timeOfToll)
        {
            switch (timeOfToll.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
                default:
                    return true;
            }
        }

    }
}
