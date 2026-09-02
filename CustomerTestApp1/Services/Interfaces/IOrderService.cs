using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResultData<List<OrderListResponseDto>>> GetAllOrdersAsync();
        Task<ResultData<OrderResponseDto>> GetOrderByOrderIdAsync(int orderId, int customerId);
        Task<ResultData<List<OrderListResponseDto>>> GetOrdersByCustomerIdAsync(int customerId);
        Task<ResultData<OrderResponseDto>> AddNewOrderAsync(OrderCreationDto dto);
        Task<ResultData<string>> UpdateOrderAsync(int orderId, int customerId, OrderUpdateDto dto);
        Task<ResultData<string>> DeletedOrderAsync(int orderId, int customerId);
    }
}
