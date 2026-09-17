using System.ComponentModel.DataAnnotations;

namespace CustomerTestApp1.DTOS;

public class CustomerCreationDto {
  [Display(Name = "First Name")]
  [Required(ErrorMessage = "First Name is Required")]
  public string FirstName { get; set; } = null!;

  [Display(Name = "Last Name")] public string? LastName { get; set; }

  public string? PhoneNumber { get; set; }

  [Display(Name = "Email Address")]
  [Required(ErrorMessage = "Email is required to register")]
  [EmailAddress(ErrorMessage = "Email Must be of Valid Format")]
  public string Email { get; set; } = null!;

  public string? Address { get; set; }

  [Display(Name = "PassWord")]
  [Required(ErrorMessage = "Password Is required to register")]
  public string Password { get; set; } = null!;

  public IFormFile? ProfilePicture { get; set; }
}

public class CustomerUpdateDto {
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Address { get; set; }
  public IFormFile? ProfilePicture { get; set; }
}

public class CustomerResponseDto {
  public int? CustomerId { get; set; }
  public string? UserId { get; set; }
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? Email { get; set; }
  public string? PhoneNumber { get; set; }
  public string? Address { get; set; }
}