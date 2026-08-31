namespace CustomerTestApp1.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int StockQuantity { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public Product Product { get; set; } = null!;

    }
}
