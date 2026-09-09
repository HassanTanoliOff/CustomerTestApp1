using CustomerTestApp1.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CustomerTestApp1.Services {
  public class TokenService {
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config) {
      _config = config;
    }

    public string GenerateAccessToken(User user) {
      var claims = new List<Claim>
      {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role?.ToString() ?? "User")
            };

      if (user.CustomerId.HasValue) {
        claims.Add(new("CustomerId", user.CustomerId.ToString()!));
      }

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
      var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _config["Jwt:Issuer"],
          audience: _config["Jwt:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:AccessTokenMinutes"]!)),
          signingCredentials: credentials
          );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken() {
      var bytes = RandomNumberGenerator.GetBytes(64);
      return Convert.ToBase64String(bytes);
    }

  }
}
