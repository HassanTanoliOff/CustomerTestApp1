namespace CustomerTestApp1.DTOS
{
    public class OrderItemCreationDto
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int ProductQuantity { get; set; }

        public decimal TotalPrice { get; set; }
    }


}
