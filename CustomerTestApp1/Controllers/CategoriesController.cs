using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{
  private readonly ICategoryService _service;

  public CategoriesController(ICategoryService service)
  {
    _service = service;
  }

  // GET: api/Category
  [HttpGet]
  public async Task<ActionResult<ApiResponse<List<CategoriesResponseDto>>>> GetAllCategories()
  {
    var result = await _service.GetAllCategoriesAsync();
    if (!result.Success)
      return ApiErrorStatus.Response<List<CategoriesResponseDto>>(result.ErrorType, result.Error);

    return Ok(
      ApiResponse<List<CategoriesResponseDto>>.SuccessResponse(result.Data, result.Message)
    );
  }

  // POST: api/Category
  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<ActionResult<ApiResponse<CategoriesResponseDto>>> AddNewCategory(
    [FromBody] CategoriesCreationDto dto
  )
  {
    var result = await _service.AddNewCategoryAsync(dto);
    if (!result.Success)
      return ApiErrorStatus.Response<CategoriesResponseDto>(result.ErrorType, result.Error);

    return StatusCode(
      201,
      ApiResponse<CategoriesResponseDto>.SuccessResponse(result.Data, result.Message)
    );
  }

  // PUT: api/Category/5
  [Authorize(Roles = "Admin")]
  [HttpPatch("{cId:int}")]
  public async Task<ActionResult<ApiResponse<string>>> UpdateCategory(
    int cId,
    [FromBody] CategoryUpdateDto dto
  )
  {
    var result = await _service.UpdateCategoryAsync(cId, dto);
    if (!result.Success)
      return ApiErrorStatus.Response<string>(result.ErrorType, result.Error);

    return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
  }

  // DELETE: api/Category/5
  [Authorize(Roles = "Admin")]
  [HttpDelete("{id:int}")]
  public async Task<ActionResult<ApiResponse<string>>> DeleteCategory(int id)
  {
    var result = await _service.DeleteCategoryAsync(id);
    if (!result.Success)
      return ApiErrorStatus.Response<string>(result.ErrorType, result.Error);

    return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
  }
}
