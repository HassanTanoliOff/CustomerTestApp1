namespace CustomerTestApp1.Models;

public class Order : ISoftDelete {
  public int OrderId { get; set; }
  public decimal TotalAmount { get; set; }
  public OrderStatus Status { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
  public bool IsDeleted { get; set; }
}

public enum OrderStatus {
  Pending = 0,
  InTransit = 1,
  Delivered = 2,
  Processing = 3,
  Shipped = 4,
  Canceled = 5
}