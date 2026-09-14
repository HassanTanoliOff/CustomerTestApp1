using System.ComponentModel.DataAnnotations;

namespace CustomerTestApp1.DTOS
{
  public class ProductCreationDto
  {
    [Display(Name = "Product Name")]
    [Required(ErrorMessage = "Product Name is Required")]
    public string productName { get; set; } = null!;

    public string? productDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "Product Quantity is required")]
    [Range(1, 1000000, ErrorMessage = "Quantity must be atleast 1 to 1 Million")]
    public int quantity { get; set; } = 1;

    [Display(Name = "Default/UnCategorized: 1, Electronics: 2, Books: 3")]
    public int categoryId { get; set; } = 1;

    [Required]
    [Range(100, 1000000, ErrorMessage = "Price must be between 100 and 1 Million")]
    public decimal price { get; set; } = 100;
  }

  public class ProductUpdateDto
  {
    public string? productName { get; set; }
    public string? productDescription { get; set; }
    public decimal? productPrice { get; set; }
    public int? quantity { get; set; }
    public int? categoryId { get; set; }
  }

  public class ProductResponseDto
  {
    public int productId { get; set; }
    public string? productName { get; set; }
    public string? productDescription { get; set; }
    public string? category { get; set; }
    public decimal price { get; set; }
    public string? StockAvailability { get; set; }
    public int quantity { get; set; }
  }
}
