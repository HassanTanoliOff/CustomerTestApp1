using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderListResponseDto>>> GetOrder()
    {
        return await _context.Orders
            .Select(o => new OrderListResponseDto
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                CustomerName = $"{o.Customer.FirstName} {o.Customer.LastName}",
                ItemsTotal = o.OrderItems.Sum(oi => oi.TotalNumber),
                TotalPrice = o.TotalAmount,
                OrderStatus = o.Status.ToString(),
            })
            .ToListAsync();
    }

    // GET: api/Order/5
    [HttpGet("{orderid}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderid)
    {
        var orderResult = _context.Orders
            .Select(o => new OrderResponseDto
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                TotalItems = o.OrderItems.Sum(o => o.TotalNumber),
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    ProductQuantity = oi.TotalNumber,
                    Price = oi.Amount
                }).ToList()
            }).ToListAsync();
        if (orderResult == null) return NotFound("No Orders yet");

        return Ok(orderResult);
    }

    // PUT: api/Order/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{orderid}")]
    public async Task<IActionResult> PutOrder(int? orderid, Order order)
    {
        if (orderid != order.OrderId)
        {
            return BadRequest();
        }

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrderExists(orderid))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Order
    [HttpPost]
    public async Task<ActionResult<OrderCreationDto>> AddOrder([FromBody] OrderCreationDto order)
    {
        if (!ModelState.IsValid) return BadRequest("Something Went Wrong");

        var customerExists = _context.Customers.Find(order.CustomerId);
        if (customerExists == null)
            return NotFound("Invalid Customer ID");

        if (!order.Items.Any())
            return BadRequest("No Items add to Order");

        var productsIds = order.Items.Select(i => i.ProductId).Distinct().ToList();

        var products = await _context.Products
            .Where(p => productsIds.Contains(p.Id))
            .ToListAsync();


        var productLookUp = products.ToDictionary(p => p.Id);

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;
        int totalNumberOfItems = 0;
        foreach (var item in order.Items)
        {
            var product = productLookUp[item.ProductId];
            var unitPrice = product.Price;
            totalNumberOfItems += item.ProductQuantity;
            totalAmount += unitPrice * item.ProductQuantity;

            orderItems.Add(
            new OrderItem
            {
                TotalNumber = item.ProductQuantity,
                ProductId = item.ProductId,
                Amount = unitPrice * item.ProductQuantity,

            });
        }


        var newOrder = new Order
        {
            CustomerId = order.CustomerId,
            TotalAmount = totalAmount,
            OrderItems = orderItems,
            Status = OrderStatus.Pending
        };

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        var result = new OrderResponseDto
        {
            OrderId = newOrder.OrderId,
            CustomerId = newOrder.CustomerId,
            TotalItems = totalNumberOfItems,
            TotalAmount = newOrder.TotalAmount,
            Items = newOrder.OrderItems.Select(oi => new OrderItemDto
            {
                ProductId = oi.ProductId,
                ProductName = productLookUp[oi.ProductId].Name,
                ProductQuantity = oi.TotalNumber,
                Price = productLookUp[oi.ProductId].Price

            }).ToList(),
        };

        return Ok(result);
    }

    // DELETE: api/Order/5
    [HttpDelete("{orderid}")]
    public async Task<IActionResult> DeleteOrder(int? orderid)
    {
        var order = await _context.Orders.FindAsync(orderid);

        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrderExists(int? orderid)
    {
        return _context.Orders.Any(e => e.OrderId == orderid);
    }
}
