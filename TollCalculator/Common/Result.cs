namespace Common
{
    public class Result<T>
    {
        public Result(ResultStatus resultStatus, T data)
        {
            ResultStatus = resultStatus;
            Data = data;
        }
        public ResultStatus ResultStatus { get; }
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
