using System;
using System.Collections.Generic;

namespace Common
{

    // This is the sketch of this class to keep it from becoming too complicated. 
    // Examples: insufficient information is provided here for the non-happy path
    // and partial success is not tracking which failed. It's a sample - fly with it.

    public interface IResult
    {
        string Message { get; }
    }

    public interface IResult<out T> : IResult
    {
        ResultStatus ResultStatus { get; }
        T Data { get; }
    }

    public class Result
    {
        public Result(ResultStatus resultStatus, string message)
        {
            ResultStatus = resultStatus;
            Message = message;
        }
        public ResultStatus ResultStatus { get; }

        public string Message { get; }
    }

    public class Result<T> : Result, IResult<T>
    {
        public static Result<T> Success(T data)
            => new Result<T>(ResultStatus.Success, data);

        public static Result<T> PartialFailure(IEnumerable<IResult<T>> failedResults)
            => new Result<T>(ResultStatus.PartialFailure, default(T), failedResults);

        public static Result<T> Failure(string message)
             => new Result<T>(ResultStatus.Failure, default(T), message: message);
        public static Result<T> Error(string message)
              => new Result<T>(ResultStatus.Error, default(T), message: message);

     public static Result<T> Exception(string message)
              => new Result<T>(ResultStatus.Exception, default(T), message: message);

        private Result(ResultStatus resultStatus, T data,
                    IEnumerable<IResult<T>> failedResults = null,
                    string message = null)
            : base(resultStatus, message)
        {
            Data = data;
            FailedResults = failedResults;
        }
        public T Data { get; }
        public IEnumerable<IResult<T>> FailedResults { get; }

   
    }

    public enum ResultStatus
    {
        Unknown = 0,
        Success,
        Failure,
        PartialFailure,
        Error,
        Exception
    }

}
