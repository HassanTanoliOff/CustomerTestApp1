using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Helpers;
using CustomerTestApp1.Models;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services;

public class ResetPassService(ApplicationDbContext context, IEmailService emailService,IPasswordHasher<User> passwordHasher) {
  public async Task<string> ForgotPasswordAsync(string email) {
    // email = email.Trim();
    Console.WriteLine($"[Password Reset] Forgot password: {email}]");
    if (string.IsNullOrWhiteSpace(email) || email.Equals("string")) return "Email is required to reset password";

    var user = await context.Users
      .Where(u => u.Email == email && u.IsDeleted == false)
      .FirstOrDefaultAsync();

    if (user == null) return "Email sent to reset password";

    var token = OTPGenerator.Generate();
      Console.WriteLine($"<><> Generated OTP: {token} <>");
    user.PasswordResetToken = token;
    user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(5); // 5 minutes expiry 

    await context.SaveChangesAsync();

    var resetLink = $"This is your reset password OTP Do not share it:{token}.";
    await emailService.SendEmailAsync(user.Email, "Reset Password OTP", resetLink);
    Console.WriteLine($"[Password Reset] Forgot password: {resetLink}");
    return "OTP sent to reset password.";
  }

  public async Task<string> ResetPasswordAsync(ResetPassDto dto) {
    var user = await context.Users
      .FirstOrDefaultAsync(u => u.PasswordResetToken == dto.Token);

    if (user is null)
      return "Invalid token";
    if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
      return "Expired token";

    user.PasswordHash = passwordHasher.HashPassword(user, dto.NewPassword);
    user.PasswordResetToken = null;
    user.PasswordResetTokenExpiry = null;
    user.DateUpdated = DateTime.UtcNow;

    await context.SaveChangesAsync();

    return "Password updated.";
  }
}