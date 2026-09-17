namespace CustomerTestApp1.Models;

public class Category : ISoftDelete {
  public int Id { get; set; }
  public string CategoryName { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public ICollection<Product> Products { get; set; } = new List<Product>();
  public bool IsDeleted { get; set; }
}