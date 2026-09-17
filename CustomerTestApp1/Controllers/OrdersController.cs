using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase {
  private readonly IOrderService _service;

  public OrdersController(IOrderService service) {
    _service = service;
  }

  // GET: api/Order
  [HttpGet]
  public async Task<ActionResult<List<OrderListResponseDto>>> GetAllOrders() {
    var result = await _service.GetAllOrdersAsync();

    if (!result.Success) return ApiErrorStatus.Response<List<OrderListResponseDto>>(result.ErrorType, result.Error);
    return Ok(ApiResponse<List<OrderListResponseDto>>.SuccessResponse(result.Data, result.Message));
  }

  [HttpGet("{customerId:int}")]
  public async Task<ActionResult<ApiResponse<List<OrderListResponseDto>>>> GetOrdersByCustomerId(
    int customerId
  ) {
    var result = await _service.GetOrdersByCustomerIdAsync(customerId);
    if (!result.Success)
      return ApiErrorStatus.Response<List<OrderListResponseDto>>(result.ErrorType, result.Error);

    return Ok(ApiResponse<List<OrderListResponseDto>>.SuccessResponse(result.Data, result.Message));
  }

  //// GET: api/Order/5
  [HttpGet("{customerId:int}/orders/{orderId:int}")]
  public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrderByOrderId(
    int orderId,
    int customerId
  ) {
    var orderResult = await _service.GetOrderByOrderIdAsync(orderId, customerId);

    if (!orderResult.Success)
      return ApiErrorStatus.Response<OrderResponseDto>(orderResult.ErrorType, orderResult.Error);

    return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(orderResult.Data, orderResult.Message));
  }

  // POST: api/Order
  [HttpPost]
  public async Task<ActionResult<ApiResponse<OrderResponseDto>>> AddOrder(
    [FromBody] OrderCreationDto order
  ) {
    var result = await _service.AddNewOrderAsync(order);
    if (!result.Success)
      return ApiErrorStatus.Response<OrderResponseDto>(result.ErrorType, result.Error);

    return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(result.Data, result.Message));
  }

  // patch
  [HttpPatch("{customerId:int}/orders/{orderId:int}")]
  public async Task<ActionResult<ApiResponse<string>>> UpdateOrder(
    int orderId,
    int customerId,
    [FromBody] OrderUpdateDto order
  ) {
    var result = await _service.UpdateOrderAsync(orderId, customerId, order);

    if (!result.Success)
      return ApiErrorStatus.Response<string>(result.ErrorType, result.Error);

    return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
  }

  [HttpDelete("{customerId}/orders/{orderId}")]
  public async Task<ActionResult<ApiResponse<string>>> DeleteOrder(int orderId, int customerId) {
    var result = await _service.DeletedOrderAsync(orderId, customerId);
    if (!result.Success)
      return ApiErrorStatus.Response<string>(result.ErrorType, result.Error);

    return Ok(ApiResponse<string>.SuccessResponse(result.Data, result.Message));
  }
}