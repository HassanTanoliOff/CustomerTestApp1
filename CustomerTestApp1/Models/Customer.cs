namespace CustomerTestApp1.Models
{
    public class Customer : ISoftDelete
    {
        public int CostumerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public bool IsDeleted { get; set; }

    }
}
