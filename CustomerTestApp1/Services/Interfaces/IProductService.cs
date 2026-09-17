using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces;

public interface IProductService {
  Task<ResultData<List<ProductResponseDto>>> GetAllProductsAsync();
  Task<ResultData<ProductResponseDto>> GetProductByIdAsync(int id);
  Task<ResultData<ProductResponseDto>> AddNewProductAsync(ProductCreationDto dto);
  Task<ResultData<ProductResponseDto>> UpdateProductAsync(int id, ProductUpdateDto dto);
  Task<ResultData<bool>> DeleteProductAsync(int id);
}