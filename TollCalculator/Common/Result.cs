using System;
using System.Collections.Generic;

namespace Common
{
    public interface IResult
    {
        string Message { get; }
    }

    public abstract class Result
    {
        protected Result(string message)
            => Message = message;

        public string Message { get; }

        public static FailResult<T> Fail<T>(string message = null)
            => new FailResult<T>(message);
        public static FailResult<T> Fail<T>(IResult innerResult, string message = null)
            => new FailResult<T>(innerResult, message);
        public static FailResult<T> Fail<T>(Exception exception, string message = null)
            => new ExceptionResult<T>(exception, message);
        public static SuccessResult<T> Success<T>(T value, string message = null)
            => new SuccessResult<T>(value);
        internal static Result<T> PartialFailure<T>(IEnumerable<FailResult<T>> partialFailures, string message = null)
            => new PartialFailResult<T>(partialFailures, message);
        public static IResult<T> Empty<T>() 
            => new EmptyResult<T>();
    }

    public interface IResult<out T> : IResult
    {
    }

    public abstract class Result<T> : Result, IResult<T>
    {
        protected Result()
            : this(null) { }
        protected Result(string message)
            : base(message) { }
    }

    public interface Success { }

    public class EmptyResult<T> : Result<T>
    {
        public EmptyResult() { }
    
    }
    public class SuccessResult< T> : Result<T>, Success
    {
        public SuccessResult(T value)
            : this(value, null) { }
        public SuccessResult(T value, string message)
            : base(message)
            => Value = value;

        public T Value { get; }
    }

    public interface Fail { }

    public class FailResult< T> : Result<T>, Fail
    {

        public FailResult(string failureMessage)
           : this(failureMessage, null) { }
        public FailResult(string failureMessage, string message)
            : base(message)
            => FailureMessage = failureMessage;
        public FailResult(IResult innerResult, string message)
        {
            InnerResult = innerResult;
            FailureMessage = message;
        }

        public string FailureMessage { get; }
        public IResult InnerResult { get; }
    }

    public class ExceptionResult<T> : FailResult<T>
    {
        public ExceptionResult(Exception exception)
            : this(exception, null) { }
        public ExceptionResult(Exception exception, string failureMessage)
            : this(exception, failureMessage, null) { }
        public ExceptionResult(Exception exception, string failureMessage, string message)
            : base(failureMessage, message)
            => Exception = exception;

        public Exception Exception { get; }
    }

    public class PartialFailResult<T> : FailResult<T>
    {
        public PartialFailResult(IEnumerable<FailResult<T>> failures)
            : this(failures, null) { }
        public PartialFailResult(IEnumerable<FailResult<T>> failures, string failureMessage)
            : this(failures, failureMessage, null) { }
        public PartialFailResult(IEnumerable<FailResult<T>> failures, string failureMessage, string message)
            : base(failureMessage, message)
            => Failures = failures;

        public IEnumerable<FailResult<T>> Failures { get; }
    }

    // This is the sketch of this class to keep it from becoming too complicated. 
    // Examples: insufficient information is provided here for the non-happy path
    // and partial success is not tracking which failed. It's a sample - fly with it.

    //public interface IResult
    //{
    //    string Message { get; }
    //}

    //public interface IResult<out T> : IResult
    //{
    //    ResultStatus ResultStatus { get; }
    //    T Data { get; }
    //}

    //public class Result
    //{
    //    public Result(ResultStatus resultStatus, string message)
    //    {
    //        ResultStatus = resultStatus;
    //        Message = message;
    //    }
    //    public ResultStatus ResultStatus { get; }

    //    public string Message { get; }
    //}

    //public class Result<T> : Result, IResult<T>
    //{
    //    public static Result<T> Success(T data)
    //        => new Result<T>(ResultStatus.Success, data);

    //    public static Result<T> PartialFailure(IEnumerable<IResult<T>> failedResults)
    //        => new Result<T>(ResultStatus.PartialFailure, default, failedResults);

    //    public static Result<T> Failure(string message)
    //         => new Result<T>(ResultStatus.Failure, default, message: message);
    //    public static Result<T> Error(string message)
    //          => new Result<T>(ResultStatus.Error, default, message: message);

    // public static Result<T> Exception(string message)
    //          => new Result<T>(ResultStatus.Exception, default(T), message: message);

    //    private Result(ResultStatus resultStatus, T data,
    //                IEnumerable<IResult<T>> failedResults = null,
    //                string message = null)
    //        : base(resultStatus, message)
    //    {
    //        Data = data;
    //        FailedResults = failedResults;
    //    }
    //    public T Data { get; }
    //    public IEnumerable<IResult<T>> FailedResults { get; }


    //}

    //public enum ResultStatus
    //{
    //    Unknown = 0,
    //    Success,
    //    Failure,
    //    PartialFailure,
    //    Error,
    //    Exception
    //}

}
