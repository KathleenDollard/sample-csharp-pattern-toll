using System;
using System.Collections.Generic;

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


        public static IResult<object> DoOperations(List<IResult<object>> partialFailures,
                Dictionary<string, object> dataBag,
                params (string, Func<IResult<object>, IResult<object>>)[] operationTuples)
        {
            IResult<object> result = null;
            foreach (var (operationName, operation) in operationTuples)
            {
                result = operation(result);
                if (result.ResultStatus != ResultStatus.Success)
                {
                    RecordIssue(result, operationName);
                    partialFailures.Add(result);
                    return result;
                }
                dataBag[operationName] = result.Data;
                Console.WriteLine($"{operationName} complete");
            }
            return result;
        }


        public  static void RecordIssue(IResult<object> result, string step)
          => Logger.Log(result.Message, Severity.Error);
    }
}
