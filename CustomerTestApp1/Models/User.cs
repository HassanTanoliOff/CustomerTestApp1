namespace CustomerTestApp1.Models;

public class User : ISoftDelete {
  public Guid Id { get; set; }
  public string Name { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Phone { get; set; } = null!;
  public string PasswordHash { get; set; } = null!;
  public UserRole? Role { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiry { get; set; }
  public string? PasswordResetToken { get; set; }
  public DateTime? PasswordResetTokenExpiry { get; set; }
  public bool IsActive { get; set; }
  public int? CustomerId { get; set; }
  public Customer? Customer { get; set; }
  public DateTime DateCreated { get; set; }
  public DateTime DateUpdated { get; set; }
  public DateTime? DeletionDate { get; set; }
  public bool IsDeleted { get; set; }
}

public enum UserRole {
  User,
  Admin
}