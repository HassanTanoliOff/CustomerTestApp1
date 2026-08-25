namespace CustomerTestApp1.DTOS
{
    public class ProductCreationDto
    {
        public string productName { get; set; } = string.Empty;
        public string productDescription { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
        public int categoryId { get; set; } = 1;
        public decimal price { get; set; } = 100;

    }

    public class ProductResponseDto
    {
        public int productId { get; set; }
        public string productName { get; set; } = string.Empty;
        public string productDescription { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public decimal price { get; set; }
        //public string availability { get; set; } = string.Empty;
        public int quantity { get; set; }
    }
}
