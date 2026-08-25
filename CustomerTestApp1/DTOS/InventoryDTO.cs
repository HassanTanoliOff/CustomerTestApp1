namespace CustomerTestApp1.DTOS
{
    public class InventoryCreationDto
    {
        public int Productid { get; set; }
        public int StockQuantity { get; set; }
    }
    public class InventoryResponseDto
    {
        public int Id { get; set; }
        public int Productid { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
        public DateTime DateCreated { get; set; }
        //public DateTime DateUpdated { get; set; }
    }
}
