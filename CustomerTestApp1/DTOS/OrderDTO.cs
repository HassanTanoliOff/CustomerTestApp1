namespace CustomerTestApp1.DTOS
{
    public class OrderCreationDto
    {
        //public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }
        public List<OrderItemCreationDto> Items { get; set; } = new();
    }
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int OrderItemsQuantity { get; set; }
        public decimal OrderTotalPrice { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();

    }

    public class OrderListResponseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ItemsQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductsDto> Items { get; set; } = new();
    }

    public class ProductsDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int number { get; set; }

    }

    public class OrderUpdateDto
    {
        public OrdersController? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();

    }

}
