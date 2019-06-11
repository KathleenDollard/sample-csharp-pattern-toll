using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Common
{
    public static class Extensions
    {
        // Not an extension to avoid confusion with example
        public static bool IsWeekDay(DateTime timeOfToll)
            => timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                _ => true,
            };

        public static IResult<T> Start<T>(this T startValue)
            => Result.Success<T>(startValue);

        public static IResult<T> IfNotFailed<TIn, T>(this IResult<TIn> currentResult,
                Func<SuccessResult<TIn>, IResult<T>> operation) 
            => currentResult is SuccessResult<TIn> successResult 
                ? operation(successResult) 
                : Result.Fail<T>(currentResult);


        private static readonly Dictionary<object, (string Name, object FuncAsObject)> cache
            = new Dictionary<object, (string Name, object FuncAsObject)>();
        public static (string name, Func<TParam, TReturn> operation) GetNameAndFunc<TParam, TReturn>(
            this Expression<Func<TParam, TReturn>> operationExpression)
        {
            if (cache.TryGetValue(operationExpression, out (string Name, object FuncAsObject) tuple))
            {
                // The following needs work because it currently returns null without warning. 
                return (tuple.Name, tuple.FuncAsObject as Func<TParam, TReturn>);
            }

            Func<TParam, TReturn> func = operationExpression.Compile();
            Expression body = operationExpression.Body;
            var name = "Unknown operation";
            switch (body)
            {
                case MethodCallExpression methodCallExpression:
                    name = methodCallExpression.Method.Name;
                    break;

            }
            cache[operationExpression] = (name, func);
            return (name, func);
        }

        private static void RecordComplete(string operationName) 
            => Logger.LogInfo($"{operationName} complete");
        public static void RecordIssue<T>(Result<T> result, string step)
            => Logger.Log(result.Message, Severity.Error);
    }
}
