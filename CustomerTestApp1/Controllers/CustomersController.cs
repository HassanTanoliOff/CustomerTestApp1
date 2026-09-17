using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController(ICustomerService service) : ControllerBase {
  private readonly ICustomerService _service = service;

  //[Authorize(Roles = "Admin")]
  [HttpGet]
  public async Task<ActionResult<ApiResponse<List<CustomerResponseDto>>>> GetAll() {
    var result = await _service.GetAllCustomersAsync();
    if (!result.Success)
      return ApiErrorStatus.Response<List<CustomerResponseDto>>(result.ErrorType, result.Error);

    return Ok(ApiResponse<List<CustomerResponseDto>>.SuccessResponse(result.Data, result.Message));
  }

  // // Add file upload to customer
  [HttpPost]
  public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> AddCustomer(
    [FromForm] CustomerCreationDto customer
  ) {
    var result = await _service.AddCustomerAsync(customer);
    if (!result.Success)
      return ApiErrorStatus.Response<CustomerResponseDto>(result.ErrorType, result.Error);

    return Ok(ApiResponse<CustomerResponseDto>.SuccessResponse(result.Data, result.Message));
  }

  [Authorize]
  [HttpPatch("{id:int}")]
  public async Task<ActionResult<ApiResponse<CustomerResponseDto>>> UpdateCustomer(
    int id,
    [FromForm] CustomerUpdateDto customerDto
  ) {
    var result = await _service.UpdateCustomerAsync(id, customerDto);
    if (!result.Success)
      return ApiErrorStatus.Response<CustomerResponseDto>(result.ErrorType, result.Error);

    return Ok(ApiResponse<CustomerResponseDto>.SuccessResponse(result.Data, result.Message));
  }

  [Authorize]
  [HttpDelete("{id:int}")]
  public async Task<ActionResult<ApiResponse<bool>>> DeleteCustomer(int id) {
    var result = await _service.DeleteCustomerAsync(id);
    if (!result.Success)
      return ApiErrorStatus.Response<bool>(result.ErrorType, result.Error);

    return Ok(ApiResponse<bool>.SuccessResponse(result.Data, result.Message));
  }
}