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

        public Result<T> Try<T>(Func<Result<T>> operation, Func<Result<T>> catchAction)
        {
            try
            {
                return operation();
            }
            catch (Exception e)
            {
                return Result<T>.Error(e.Message);
            }
        }

        public static IResult<object> TryGet(params Func<IResult<object>>[] tryOperations)
        {
            foreach (var tryOperation in tryOperations)
            {
                var result = tryOperation();
                if (result.ResultStatus == ResultStatus.Success)
                {
                    return result;
                }
            }
            return Result<object>.Failure("Could not find item");
        }

    }
}
