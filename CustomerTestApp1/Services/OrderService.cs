using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultData<List<OrderListResponseDto>>> GetAllOrdersAsync()
        {
            try
            {

                List<OrderListResponseDto> orders = await _context.Orders
                    .AsNoTracking()
                    .Where(or => or.IsDeleted == false)
                    .Select(or => new OrderListResponseDto
                    {
                        OrderId = or.OrderId,
                        CustomerId = or.CustomerId,
                        CustomerName = $"{or.Customer.FirstName} {or.Customer.LastName}",
                        ItemsQuantity = or.OrderItems.Sum(oi => oi.TotalNumber),
                        TotalPrice = or.OrderItems.Sum(oi => oi.Amount * oi.TotalNumber),
                        Items = or.OrderItems.Select(oi => new ProductsDto
                        {
                            ProductId = oi.ProductId,
                            Name = oi.Product.Name,
                            number = oi.TotalNumber
                        }).ToList(),
                    })
                    .ToListAsync();

                if (orders.Count == 0)
                    return ResultData<List<OrderListResponseDto>>.Pass(orders, "No orders in the list");

                return ResultData<List<OrderListResponseDto>>.Pass(orders, "All orders Retrieved");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug start] Type: {ex.GetType().Name} , Message: {ex.Message}, Inner: {ex.InnerException?.Message} [Debug end]");
                return ResultData<List<OrderListResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
            }

        }

        public Task<ResultData<List<OrderListResponseDto>>> GetOrdersByCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<OrderResponseDto>> GetOrderByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }


        public Task<ResultData<OrderResponseDto>> AddNewOrderAsync(OrderCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<string>> UpdateOrderAsync(int orderId, int customerId, OrderCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<string>> DeletedOrderAsync(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
