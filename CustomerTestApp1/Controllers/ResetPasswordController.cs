using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ResetPasswordController(ResetPassService service) : ControllerBase {
  
  [HttpPost("forgot")]
  public async Task<ApiResponse<string>> ForgotPassword(string email) {
    var result = await service.ForgotPasswordAsync(email);
    return ApiResponse<string>.SuccessResponse(result);
  }

  [HttpPost("reset")]       
  public async Task<ApiResponse<string>> ResetPassword(ResetPassDto dto) {
    var result = await service.ResetPasswordAsync(dto);
    return ApiResponse<string>.SuccessResponse(result);
  }
}