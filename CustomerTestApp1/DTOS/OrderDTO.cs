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
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();


    }

    public class OrderListResponseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ItemsTotal { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }

    }
}
