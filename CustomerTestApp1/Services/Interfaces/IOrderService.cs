using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ResultData<List<OrderListResponseDto>>> GetAllOrdersAsync();
        Task<ResultData<OrderResponseDto>> GetOrderByOrderIdAsync(int orderId);
        Task<ResultData<List<OrderListResponseDto>>> GetOrdersByCustomerId(int customerId);
        Task<ResultData<OrderResponseDto>> AddNewOrderAsync(OrderCreationDto dto);
        Task<ResultData<string>> UpdateOrderAsync(int orderId, int customerId, OrderCreationDto dto);
        Task<ResultData<string>> DeletedOrderAsync(int orderId);
    }
}
