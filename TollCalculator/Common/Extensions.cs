using System;

namespace Common
{
    public class Extensions
    {
        // Not an extension to avoid confusion with example
        public static bool IsWeekDay(DateTime timeOfToll)
            => timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true,
            };

        public static IResult<T> DoUntilSuccess<T>(params Func<IResult<T>>[] tryOperations)
        {
            foreach (var tryOperation in tryOperations)
            {
                var result = tryOperation();
                if (result.ResultStatus == ResultStatus.Success)
                {
                    return result;
                }
            }
            return Result<T>.Failure("Nothing found");
        }
    }
}
