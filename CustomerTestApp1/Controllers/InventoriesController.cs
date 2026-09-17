using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase {
  private readonly IInventoryService _service;

  public InventoriesController(IInventoryService service) {
    _service = service;
  }

  [Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<ActionResult<ApiResponse<List<InventoryResponseDto>>>> GetInventory() {
    var result = await _service.GetInventoryAsync();

    if (!result.Success)
      return ApiErrorStatus.Response<List<InventoryResponseDto>>(result.ErrorType, result.Error);

    return Ok(ApiResponse<List<InventoryResponseDto>>.SuccessResponse(result.Data, result.Message));
  }
}