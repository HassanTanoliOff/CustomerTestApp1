namespace CustomerTestApp1.DTOS;

public record LoginDto(string Email, string Password);

public record RefreshRequestDto(string RefreshToken);

public record AuthResponseDto(string AccessToken, string RefreshToken);