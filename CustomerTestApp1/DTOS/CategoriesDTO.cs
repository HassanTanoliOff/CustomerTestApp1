namespace CustomerTestApp1.DTOS;

public class CategoriesCreationDto {
  public string CategoryName { get; set; } = string.Empty;
}

public class CategoriesResponseDto {
  public int CategoryId { get; set; }
  public string CategoryName { get; set; } = string.Empty;
  public DateTime CategoryCreatedDate { get; set; }
  public DateTime CategoryModifiedDate { get; set; }
  public string Active { get; set; } = string.Empty;
}

public class CategoryUpdateDto {
  public string? CategoryName { get; set; }
}