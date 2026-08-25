using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerTestApp1.Models
{
    public class Product
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; } = 1;
        public decimal Price { get; set; }
        public Inventory? Inventory { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        private const int minimumStock = 1;

        [NotMapped]
        public string StockAvailability =>
            (Inventory?.StockQuantity ?? 0) >= minimumStock
            ? "In Stock" : "Out of Stock";
    }

    //public enum StockAvailability
    //{
    //    OutOfStock = 0,
    //    InStock = 1,

    //}
}
