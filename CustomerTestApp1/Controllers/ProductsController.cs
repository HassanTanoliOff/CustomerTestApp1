using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{

    private readonly IProductService _service;
    public ProductsController(IProductService service)
    {

        _service = service;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ProductResponseDto>>>> GetAllProducts()
    {
        var result = await _service.GetAllProductsAsync();
        if (!result.Success)
            return ApiErrorStatus.Response<List<ProductResponseDto>>(result.ErrorType, result.Error);

        return Ok(ApiResponse<List<ProductResponseDto>>.SuccessResponse(result.Data, "Products Retrieved Successfully"));
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> GetProductById(int id)
    {
        var result = await _service.GetProductByIdAsync(id);
        if (!result.Success)
            return ApiErrorStatus.Response<ProductResponseDto>(result.ErrorType, result.Error);

        return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(result.Data, "Product Retrieved"));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> AddProduct([FromBody] ProductCreationDto dto)
    {
        var result = await _service.AddNewProductAsync(dto);
        if (!result.Success)
            return ApiErrorStatus.Response<ProductResponseDto>(result.ErrorType, result.Error);

        return ApiResponse<ProductResponseDto>.SuccessResponse(result.Data, "New Product added.");
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<ApiResponse<ProductResponseDto>>> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
    {
        var result = await _service.UpdateProductAsync(id, dto);

        if (!result.Success)
            return ApiErrorStatus.Response<ProductResponseDto>(result.ErrorType, result.Error);

        return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(result.Data, "Product Updated."));
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
    {
        var result = await _service.DeleteProductAsync(id);

        if (!result.Success)
            return ApiErrorStatus.Response<bool>(result.ErrorType, result.Error);

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Product Deleted"));
    }


}
