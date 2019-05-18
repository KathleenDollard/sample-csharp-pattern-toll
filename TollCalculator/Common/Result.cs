namespace Common
{
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
            => new Result<T>(ResultStatus.Success, data, null);

        public static Result<T> Failure(string message)
             => new Result<T>(ResultStatus.Failure, default(T), message);

        private Result(ResultStatus resultStatus, T data, string message)
            : base (resultStatus, message)
        {
            Data = data;
        }
        public T Data { get; }
    }

    public enum ResultStatus
    {
        Unknown = 0,
        Success,
        Failure,
        PartialFailure,
        Error
    }

}
