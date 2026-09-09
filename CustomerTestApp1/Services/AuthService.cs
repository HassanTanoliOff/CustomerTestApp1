using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Logs;
using CustomerTestApp1.Models;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services
{
  public class AuthService : IAuthService
  {
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<User> _passHasher;
    private readonly TokenService _tokenService;
    private readonly IConfiguration _config;

    //private Microsoft.AspNetCore.Identity.PasswordVerificationResult PasswordVerificationResult;

    public AuthService(
      ApplicationDbContext context,
      IPasswordHasher<User> hasher,
      TokenService service,
      IConfiguration config
    )
    {
      _context = context;
      _passHasher = hasher;
      _tokenService = service;
      _config = config;
    }

    public async Task<ResultData<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
      try
      {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
          u.Email == dto.Email && !u.IsDeleted
        );
        if (user is null)
          return ResultData<AuthResponseDto>.Fail(
            "Invalid Credentials",
            ResultErrorType.UnAuthorized
          );

        var verifyPass = _passHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (verifyPass == PasswordVerificationResult.Failed)
          return ResultData<AuthResponseDto>.Fail(
            "Invalid Credentials",
            ResultErrorType.UnAuthorized
          );

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
          double.Parse(_config["Jwt:RefreshTokenDays"]!)
        );

        await _context.SaveChangesAsync();

        return ResultData<AuthResponseDto>.Pass(new AuthResponseDto(accessToken, refreshToken));
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<AuthResponseDto>.Fail(ex.Message, ResultErrorType.UnAuthorized);
      }
    }

    public async Task<ResultData<AuthResponseDto>> RefreshAsync(RefreshRequestDto dto)
    {
      try
      {
        var user = await _context.Users.FirstOrDefaultAsync(u =>
          u.RefreshToken == dto.RefreshToken && !u.IsDeleted
        );

        if (user is null || user.RefreshTokenExpiry < DateTime.UtcNow)
          return ResultData<AuthResponseDto>.Fail(
            "Invalid or Token Expired",
            ResultErrorType.UnAuthorized
          );

        var newAccessToken = _tokenService.GenerateAccessToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(
          double.Parse(_config["Jwt:RefreshTokenDays"]!)
        );

        await _context.SaveChangesAsync();

        return ResultData<AuthResponseDto>.Pass(
          new AuthResponseDto(newAccessToken, newRefreshToken)
        );
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<AuthResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
      }
    }
  }
}
