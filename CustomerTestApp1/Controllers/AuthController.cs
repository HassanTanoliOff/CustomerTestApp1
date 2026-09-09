using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService service)
    {
      _authService = service;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto dto)
    {
      var result = await _authService.LoginAsync(dto);
      if (!result.Success)
        return ApiErrorStatus.Response<AuthResponseDto>(result.ErrorType, result.Error);

      return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result.Data!, result.Message));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Refresh(RefreshRequestDto dto)
    {
      var result = await _authService.RefreshAsync(dto);
      if (!result.Success)
        return ApiErrorStatus.Response<AuthResponseDto>(result.ErrorType, result.Error);

      return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result.Data!, result.Message));
    }
  }
}
