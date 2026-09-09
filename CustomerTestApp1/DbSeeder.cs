using CustomerTestApp1.Data;
using CustomerTestApp1.Logs;
using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1
{
  public static class DbSeeder
  {
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
      using var scope = serviceProvider.CreateScope();

      var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
      var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

      bool adminExists = await context.Users.AnyAsync(u => u.Role == UserRole.Admin);
      if (adminExists)
        return;

      var adminUser = new User
      {
        Id = new Guid(),
        Name = "admin",
        Email = "Admin123@email.com",
        Role = UserRole.Admin,
        Phone = "090078601",
        IsActive = true,
        IsDeleted = false,
        DateCreated = DateTime.UtcNow,
        DateUpdated = DateTime.UtcNow,
      };

      adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin@123");

      try
      {
        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
      }
    }
  }
}
