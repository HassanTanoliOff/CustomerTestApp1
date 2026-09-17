namespace CustomerTestApp1.Models;

public class OrderItem : ISoftDelete {
  public int Id { get; set; }
  public int TotalNumber { get; set; }
  public decimal Amount { get; set; }
  public int ProductId { get; set; }
  public Product Product { get; set; } = null!;
  public int OrderId { get; set; }
  public Order Order { get; set; } = null!;
  public bool IsDeleted { get; set; }
}