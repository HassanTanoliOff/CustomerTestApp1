using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Logs;
using CustomerTestApp1.Models;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services;

public class OrderService : IOrderService {
  private readonly ApplicationDbContext _context;

  public OrderService(ApplicationDbContext context) {
    _context = context;
  }


  public async Task<ResultData<List<OrderListResponseDto>>> GetAllOrdersAsync() {
    try {
      var orders = await _context.Orders
        .AsNoTracking()
        .Where(or => or.IsDeleted == false)
        .Select(or => new OrderListResponseDto {
          OrderId = or.OrderId,
          CustomerId = or.CustomerId,
          CustomerName = $"{or.Customer.FirstName} {or.Customer.LastName}",
          ItemsQuantity = or.OrderItems.Sum(oi => oi.TotalNumber),
          TotalPrice = or.OrderItems.Sum(oi => oi.Amount * oi.TotalNumber),
          Items = or.OrderItems.Select(oi => new ProductsDto {
            ProductId = oi.ProductId,
            Name = oi.Product.Name,
            number = oi.TotalNumber
          }).ToList()
        })
        .ToListAsync();

      if (orders.Count == 0)
        return ResultData<List<OrderListResponseDto>>.Pass(orders, "No orders in the list");

      return ResultData<List<OrderListResponseDto>>.Pass(orders, "All orders Retrieved");
    } catch (Exception ex) {
      Console.WriteLine(
        $"[Debug start] Type: {ex.GetType().Name} , Message: {ex.Message}, Inner: {ex.InnerException?.Message} [Debug end]");
      return ResultData<List<OrderListResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }


  public async Task<ResultData<List<OrderListResponseDto>>> GetOrdersByCustomerIdAsync(int customerId) {
    var customerExists = await _context.Customers
      .Where(c => c.IsDeleted == false)
      .FirstOrDefaultAsync(c => c.CostumerId == customerId);

    if (customerExists == null)
      return ResultData<List<OrderListResponseDto>>.Fail($"Customer with the Id:{customerId} Does not Exists.",
        ResultErrorType.UnAuthorized);

    try {
      var orders = await _context.Orders
        .AsNoTracking()
        .Where(or => or.CustomerId == customerExists.CostumerId)
        .Where(or => or.IsDeleted == false)
        .Select(or => new OrderListResponseDto {
          OrderId = or.OrderId,
          CustomerId = customerId,
          CustomerName = $"{or.Customer.FirstName} {or.Customer.LastName}",
          ItemsQuantity = or.OrderItems.Sum(oi => oi.TotalNumber),
          TotalPrice = or.OrderItems.Sum(oi => oi.Amount * oi.TotalNumber),
          Items = or.OrderItems.Select(oi => new ProductsDto {
            ProductId = oi.ProductId,
            Name = oi.Product.Name,
            number = oi.TotalNumber
          }).ToList()
        })
        .ToListAsync();

      if (orders.Count == 0)
        return ResultData<List<OrderListResponseDto>>.Pass(orders, "Customer has no Orders yet.");

      return ResultData<List<OrderListResponseDto>>.Pass(orders,
        $"All orders for Customer with Id: {customerExists.CostumerId} were Retrieved.");
    } catch (Exception ex) {
      Console.WriteLine(
        $" [Debug_start] Type: {ex.GetType().Name}, Message: {ex.Message} , Inner: {ex.InnerException?.Message} [Debug_end]");

      return ResultData<List<OrderListResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }


  public async Task<ResultData<OrderResponseDto>> GetOrderByOrderIdAsync(int orderId, int customerId) {
    var customerExists = await _context.Customers
      .AsNoTracking()
      .AnyAsync(c => c.CostumerId == customerId && c.IsDeleted == false);

    if (!customerExists)
      return ResultData<OrderResponseDto>.Fail($"Customer with the Id:{customerId} Does not exists.",
        ResultErrorType.NotFound);
    try {
      var order = await _context.Orders
        .AsNoTracking()
        .Where(or => or.CustomerId == customerId && or.OrderId == orderId && or.IsDeleted == false)
        .Select(or => new OrderResponseDto {
          OrderId = or.OrderId,
          CustomerId = or.CustomerId,
          OrderItemsQuantity = or.OrderItems.Sum(oi => oi.TotalNumber),
          OrderTotalPrice = or.OrderItems.Sum(oi => oi.TotalNumber * oi.Amount),
          Items = or.OrderItems.Select(oi => new OrderItemDto {
            ProductId = oi.ProductId,
            ProductName = oi.Product.Name,
            UnitPrice = oi.Amount,
            ProductQuantity = oi.TotalNumber,
            TotalPrice = oi.TotalNumber * oi.Amount
          }).ToList()
        })
        .FirstOrDefaultAsync();

      if (order == null)
        return ResultData<OrderResponseDto>.Fail($"Order with the Id:{orderId} Not found or was deleted.",
          ResultErrorType.NotFound);

      return ResultData<OrderResponseDto>.Pass(order, "Order Retrieved");
    } catch (Exception ex) {
      CatchExceptionConsoleLog.Debug(ex);

      return ResultData<OrderResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }


  public async Task<ResultData<OrderResponseDto>> AddNewOrderAsync(OrderCreationDto dto) {
    if (dto.Items.Count == 0)
      return ResultData<OrderResponseDto>.Fail("Order list is empty add items to order.");

    var dtoItems = dto.Items.ToList();
    var emptyItemMessage = string.Empty;
    foreach (var item in dtoItems)
      if (item.ProductQuantity <= 0)
        emptyItemMessage += $"Id:{item.ProductId},";

    if (emptyItemMessage.Length > 0) {
      var message = "Item(s) quantity empty on ," + emptyItemMessage;
      return ResultData<OrderResponseDto>.Fail(message);
    }

    var customerExists = await _context.Customers
      .AsNoTracking()
      .AnyAsync(c => c.CostumerId == dto.CustomerId && c.IsDeleted == false);

    if (!customerExists)
      return ResultData<OrderResponseDto>.Fail("Failed to Place Order (CustomerId might be incorrect or was deleted).",
        ResultErrorType.NotFound);


    var itemsIds = dto.Items.Select(i => i.ProductId).Distinct().ToList();


    var productsFromDb = await _context.Products
      .Include(p => p.Inventory)
      .Include(p => p.Category)
      .Where(p => itemsIds.Contains(p.Id) && p.IsDeleted == false)
      .ToListAsync();

    var productLookUp = productsFromDb.ToDictionary(p => p.Id);

    var newOrderItems = new List<OrderItem>();


    foreach (var item in dto.Items) {
      //var product = productLookUp[item.ProductId];
      if (!productLookUp.TryGetValue(item.ProductId, out var product))
        return ResultData<OrderResponseDto>.Fail($"Product with Id:{item.ProductId} Not Found Or was deleted",
          ResultErrorType.NotFound);

      var unitPrice = product.Price;

      newOrderItems.Add(new OrderItem {
        ProductId = product.Id,
        TotalNumber = item.ProductQuantity,
        Amount = unitPrice * item.ProductQuantity
      });
    }

    var newOrder = new Order {
      CustomerId = dto.CustomerId,
      Status = OrderStatus.Pending,
      TotalAmount = newOrderItems.Sum(oi => oi.Amount),
      OrderItems = newOrderItems
    };

    try {
      _context.Orders.Add(newOrder);

      await _context.SaveChangesAsync();

      var orderResponse = new OrderResponseDto {
        OrderId = newOrder.OrderId,
        CustomerId = newOrder.CustomerId,
        OrderItemsQuantity = newOrder.OrderItems.Sum(oi => oi.TotalNumber),
        OrderTotalPrice = newOrder.OrderItems.Sum(oi => oi.Amount),
        Items = newOrder.OrderItems.Select(oi => new OrderItemDto {
          ProductId = oi.ProductId,
          ProductName = oi.Product.Name,
          UnitPrice = oi.Amount,
          ProductQuantity = oi.TotalNumber,
          TotalPrice = oi.Amount * oi.TotalNumber
        }).ToList()
      };

      return ResultData<OrderResponseDto>.Pass(orderResponse, "Order Created");
    } catch (Exception ex) {
      CatchExceptionConsoleLog.Debug(ex);
      return ResultData<OrderResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }


  public async Task<ResultData<string>> UpdateOrderAsync(int orderId, int customerId, OrderUpdateDto dto) {
    var customerExists = await _context.Customers
      .AsNoTracking()
      .AnyAsync(c => c.CostumerId == customerId && c.IsDeleted == false);

    var orderExists = await _context.Orders
      .AsNoTracking()
      .AnyAsync(or => or.OrderId == orderId && or.IsDeleted == false);


    if (!customerExists)
      return ResultData<string>.Fail("Unable to update (Customer Id might be incorrect.)", ResultErrorType.NotFound);

    if (!!orderExists)
      return ResultData<string>.Fail("Unable to update, Order Id might be incorrect.");

    var order = await _context.Orders
      .Include(or => or.OrderItems)
      .FirstOrDefaultAsync(or => or.OrderId == orderId);

    if (order is null) {
      // do something in case if order is null ?? 
    }

    var existingOrderItems = order?.OrderItems.ToDictionary(oi => oi.ProductId);
    var incomingOrderItems = dto.OrderItems.ToDictionary(oi => oi.ProductId);


    /// lets get all the products from db that are present in new/ incoming items

    var productsFromDb = await _context.Products
      .AsNoTracking()
      .Include(p => p.Inventory)
      .Where(p => incomingOrderItems.ContainsKey(p.Id) && p.IsDeleted == false)
      .ToDictionaryAsync(p => p.Id);

    // Compare existing order form new/updated order and separate items that 
    // needs to increase or decrease or be removed
    // and items that were newly added to order 


    var newItemsToAdd = incomingOrderItems.Values
      .Where(ie => !existingOrderItems.ContainsKey(ie.ProductId))
      .ToList();

    //
    //First lets try to update the items that already exists 
    // 


    foreach (var item in incomingOrderItems.Values.Except(newItemsToAdd)) {
      var existingItem = existingOrderItems?.Values
        .FirstOrDefault(ei => ei.ProductId == item.ProductId);
      var itemDataFromDb = productsFromDb.Values.FirstOrDefault(p => p.Id == item.ProductId);

      if (itemDataFromDb is null)
        return ResultData<string>
          .Fail($"The product :{item.ProductName} can not be added as it was deleted by the admin.",
            ResultErrorType.NotFound);
      if (item.ProductQuantity <= 0) {
        ///  this items has now quantity = 0 so 
        ///  we take this from existing order item with same id and update it to isDeleted = true

        existingItem?.IsDeleted = true;
        continue;
      }

      if (item.ProductQuantity >= 1 && item.ProductQuantity <= 100) {
        existingItem.TotalNumber = item.ProductQuantity;
        existingItem.Amount = item.ProductQuantity * itemDataFromDb.Price;
        existingItem.IsDeleted = false;
      }
    }


    // this was for update now new items 

    foreach (var item in newItemsToAdd) {
      var itemInfoFromDb = productsFromDb.Values.FirstOrDefault(p => p.Id == item.ProductId);
      var unitPrice = itemInfoFromDb.Price;
      order.OrderItems.Add(new OrderItem {
        ProductId = item.ProductId,
        TotalNumber = item.ProductQuantity,
        Amount = unitPrice * item.ProductQuantity,
        IsDeleted = false
      });
    }


    try {
      await _context.SaveChangesAsync();

      return ResultData<string>.Pass($" Order ID:{orderId}", "Order updated.");
    } catch (Exception ex) {
      CatchExceptionConsoleLog.Debug(ex);
      return ResultData<string>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }


  public async Task<ResultData<string>> DeletedOrderAsync(int orderId, int customerId) {
    var customerExists = await _context.Customers
      .AnyAsync(c => c.CostumerId == customerId && c.IsDeleted == false);

    if (!customerExists)
      return ResultData<string>.Fail($"Customer with Id:{customerId} not found.", ResultErrorType.NotFound);

    var order = await _context.Orders
      .FirstOrDefaultAsync(or => or.OrderId == orderId && or.CustomerId == customerId && or.IsDeleted == false);

    if (order is null)
      return ResultData<string>.Fail($"Order with Id: {orderId} does not exists", ResultErrorType.NotFound);

    order.IsDeleted = true;
    try {
      await _context.SaveChangesAsync();

      return ResultData<string>.Pass($"order id:{orderId}", "Order Deleted");
    } catch (Exception ex) {
      CatchExceptionConsoleLog.Debug(ex);
      return ResultData<string>.Fail(ex.Message, ResultErrorType.Unknown);
    }
  }
}