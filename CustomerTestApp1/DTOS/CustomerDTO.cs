using System.ComponentModel.DataAnnotations;

namespace CustomerTestApp1.DTOS
{
    public class CustomerCreationDto
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email is required to register")]
        [EmailAddress(ErrorMessage = "Email Must be of Valid Format")]
        public string Email { get; set; } = null!;
        public string? Address { get; set; }
    }

    public class CustomerUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

    }

    public class CustomerResponseDto
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Address { get; set; } = string.Empty;
    }

}
