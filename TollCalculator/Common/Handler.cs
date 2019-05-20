using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public class Handler
    {
        public static IResult<T> Try<T>(Func<IResult<T>> operation)
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
    }
}
