using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
   public static class Handler
    {
        public static Result<T> Try<T>(Func<Result<T>> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Result.Fail<T>(e.Message);
            }
        }
    }
}
