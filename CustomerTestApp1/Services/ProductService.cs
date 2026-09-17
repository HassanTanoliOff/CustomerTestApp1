using System.Text.Json;
using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services;

public class ProductService : IProductService {
  private readonly ApplicationDbContext _context;

  public ProductService(ApplicationDbContext context) {
    _context = context;
  }

  public async Task<ResultData<List<ProductResponseDto>>> GetAllProductsAsync() {
    try {
      var products = await _context
        .Products.AsNoTracking()
        .Where(p => p.IsDeleted == false)
        .Select(p => new {
          p.Id,
          p.Name,
          p.Description,
          Category = p.Category.CategoryName,
          p.Price,
          Quantity = p.Inventory.StockQuantity
        })
        .OrderBy(p => p.Name)
        .ToListAsync();

      var result = products
        .Select(p => new ProductResponseDto {
          productId = p.Id,
          productName = p.Name,
          productDescription = p.Description,
          category = p.Category,
          price = p.Price,
          quantity = p.Quantity,
          StockAvailability = p.Quantity >= 1 ? "In Stock " : "Out of Stock"
        })
        .ToList();

      if (products.Count == 0)
        return ResultData<List<ProductResponseDto>>.Pass(
          result,
          "No Products yet or Products list is empty or were deleted."
        );

      return ResultData<List<ProductResponseDto>>.Pass(result, "All Products Retrieved");
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug] Type: {ex.GetType().Name} , Message: {ex.Message}, Inner: {ex.InnerException?.Message}"
      );
      return ResultData<List<ProductResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }

  public async Task<ResultData<ProductResponseDto>> GetProductByIdAsync(int id) {
    try {
      var product = await _context
        .Products.Where(p => p.Id == id)
        .AsNoTracking()
        .Where(p => p.IsDeleted == false)
        .Select(p => new ProductResponseDto {
          productId = p.Id,
          productName = p.Name,
          productDescription = p.Description,
          category = p.Category.CategoryName,
          price = p.Price,
          StockAvailability = p.StockAvailability,
          quantity = p.Inventory.StockQuantity
        })
        .FirstOrDefaultAsync();

      if (product == null)
        return ResultData<ProductResponseDto>.Fail(
          $"No Product with Id:{id} was Found.",
          ResultErrorType.NotFound
        );

      return ResultData<ProductResponseDto>.Pass(product, "Product found.");
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug] Type: {ex.GetType().Name}, Message: {ex.Message}, Inner:{ex.InnerException?.Message}"
      );
      return ResultData<ProductResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }

  public async Task<ResultData<ProductResponseDto>> AddNewProductAsync(ProductCreationDto dto) {
    var CategoryIdFromDto = dto.categoryId == 0 ? 1 : dto.categoryId;

    var formattedProductName = dto.productName.Trim();

    var newProduct = new Product {
      Name = formattedProductName,
      Description = dto.productDescription.Trim(),
      Price = dto.price,
      CategoryId = CategoryIdFromDto,
      Inventory = new Inventory { StockQuantity = dto.quantity }
    };
    Console.WriteLine($"[Debug] at newProduct {JsonSerializer.Serialize(dto)}");

    try {
      var productCreated = await _context.Products.AddAsync(newProduct);
      await _context.SaveChangesAsync();

      var category = await _context
        .Categories.Where(c => c.Id == CategoryIdFromDto)
        .Select(c => new { c.Id, c.CategoryName })
        .FirstOrDefaultAsync();

      var result = new ProductResponseDto {
        productId = productCreated.Entity.Id,
        productName = productCreated.Entity.Name,
        productDescription = productCreated.Entity.Description,
        price = productCreated.Entity.Price,
        category = category?.CategoryName,
        quantity = productCreated.Entity.Inventory.StockQuantity,
        StockAvailability = productCreated.Entity.StockAvailability
      };
      return ResultData<ProductResponseDto>.Pass(result, "Product Created.");
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug start]=> Type: {ex.GetType().Name} , Message: {ex.Message} , inner: {ex.InnerException?.Message}.[Debug end]"
      );
      return ResultData<ProductResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }

  public async Task<ResultData<ProductResponseDto>> UpdateProductAsync(
    int id,
    ProductUpdateDto dto
  ) {
    var productExists = await _context
      .Products.Include(p => p.Inventory)
      .Include(p => p.Category)
      .Where(p => p.Id == id)
      .Where(p => p.IsDeleted == false)
      .FirstOrDefaultAsync();

    if (productExists == null)
      return ResultData<ProductResponseDto>.Fail(
        "No Product was found or was Deleted.",
        ResultErrorType.NotFound
      );

    productExists.Name =
      string.IsNullOrWhiteSpace(dto.productName) || dto.productName.Contains("string")
        ? productExists.Name
        : dto.productName.Trim();

    productExists.Description =
      string.IsNullOrWhiteSpace(dto.productDescription)
      || dto.productDescription.Contains("string")
        ? productExists.Description
        : dto.productDescription;

    //productExists.Price = dto.productPrice ?? productExists.Price;
    productExists.Price = Convert.ToDecimal(
      dto.productPrice!.Value <= 0 ? productExists.Price : dto.productPrice
    );

    if (dto.categoryId.HasValue)
      if (dto.categoryId.Value > 0)
        productExists.CategoryId = dto.categoryId.Value;
    if (dto.quantity.HasValue)
      if (dto.quantity.Value >= 0)
        productExists.Inventory.StockQuantity = dto.quantity.Value;

    try {
      var rowsUpdated = await _context.SaveChangesAsync();

      if (rowsUpdated == 0)
        return ResultData<ProductResponseDto>.Fail(
          "Failed To update Product"
        );

      var updatedPro = new ProductResponseDto {
        productId = productExists.Id,
        productName = productExists.Name,
        productDescription = productExists.Description,
        price = productExists.Price,
        category = productExists.Category.CategoryName,
        quantity = productExists.Inventory.StockQuantity,
        StockAvailability = productExists.StockAvailability
      };

      return ResultData<ProductResponseDto>.Pass(
        updatedPro,
        $"Product with Id:{id} and was Updated."
      );
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug] Type: {ex.GetType().Name} , Message: {ex.Message}, Inner: {ex.InnerException?.Message}."
      );
      return ResultData<ProductResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }

  public async Task<ResultData<bool>> DeleteProductAsync(int id) {
    var productExists = await _context.Products.FirstOrDefaultAsync();

    if (productExists == null)
      return ResultData<bool>.Fail("Product not found.", ResultErrorType.NotFound);

    productExists.IsDeleted = true;

    try {
      var rowsEffected = await _context.SaveChangesAsync();

      if (rowsEffected == 0)
        return ResultData<bool>.Fail("Failed To Delete Product.", ResultErrorType.Conflict);

      return ResultData<bool>.Pass(
        true,
        $"Product ID:{productExists.Id} was deleted successfully"
      );
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug] Type: {ex.GetType().Name} , Message: {ex.Message} , Inner: {ex.InnerException?.Message}"
      );
      return ResultData<bool>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }
}