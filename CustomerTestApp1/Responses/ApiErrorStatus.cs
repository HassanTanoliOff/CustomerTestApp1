using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Responses
{
    public static class ApiErrorStatus
    {
        public static ActionResult Response<T>(ResultErrorType errorType, string error)
        {
            var response = ApiResponse<T>.FailResponse(error);

            return errorType switch
            {
                ResultErrorType.NotFound => new NotFoundObjectResult(response),
                ResultErrorType.Conflict => new ConflictObjectResult(response),
                ResultErrorType.UnAuthorized => new UnauthorizedObjectResult(response),
                ResultErrorType.Validation => new BadRequestObjectResult(response),
                _ => new BadRequestObjectResult(response)
            };

        }
    }
}
