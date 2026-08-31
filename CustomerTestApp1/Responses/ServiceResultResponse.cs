namespace CustomerTestApp1.Responses
{
    public class ResultData<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? Error { get; set; }
        public T? Data { get; set; }
        public ResultErrorType ErrorType { get; set; }

        public static ResultData<T> Pass(T? data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };

        public static ResultData<T> Fail(string error, ResultErrorType errorType = ResultErrorType.Validation)
            => new() { Success = false, Error = error, ErrorType = errorType };
    }

    public enum ResultErrorType
    {
        None,
        Unknown,
        NotFound,
        Validation,
        Conflict,
        UnAuthorized
    }
}

