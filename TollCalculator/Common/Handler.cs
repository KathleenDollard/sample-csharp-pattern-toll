using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
   public static class Handler
    {
        public static IResult<T> Try<T>(Func<IResult<T>> operation)
        {
            try
            {
                return operation();
            }
            catch (Exception e)
            {
                Logger.LogException(e);
                return Result<T>.Exception(e.Message);
            }
        }
    }
}
