using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomerTestApp1.Auths;

public class PassWordHasher : IPasswordHasher<User> {
  private readonly PasswordHasher<User> _hasher = new();

  public string HashPassword(User user, string password) {
    //var hasher = new PasswordHasher<User>();
    // return a sting 
    return _hasher.HashPassword(user, password);
  }

  public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword) {
    // /*var hasher = new PasswordHasher<User>();
    // return a Enumerable of password varification result*/ 
    return _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
  }
}