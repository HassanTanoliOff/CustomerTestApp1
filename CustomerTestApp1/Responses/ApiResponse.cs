namespace CustomerTestApp1.Responses;

public class ApiResponse<T> {
  public bool Success { get; set; }
  public string Message { get; set; }
  public string? Error { get; set; }
  public T? Data { get; set; }

  public static ApiResponse<T> SuccessResponse(T data, string message = "Success") {
    return new ApiResponse<T> { Success = true, Message = message, Data = data };
  }

  public static ApiResponse<T> FailResponse(string error, string message = "Failed") {
    return new ApiResponse<T> { Success = false, Message = message, Error = error };
  }
}