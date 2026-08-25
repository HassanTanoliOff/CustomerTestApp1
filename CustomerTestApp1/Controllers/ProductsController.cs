using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetProduct()
    {
        var products = await _context.Products
            .Select(p => new ProductResponseDto
            {
                productId = p.Id,
                productName = p.Name,
                productDescription = p.Description,
                price = p.Price,
                quantity = p.Inventory.StockQuantity,
                category = p.Category.CategoryName,

            }).ToListAsync();

        return Ok(products);

    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var productExists = await _context.Products
            .Include(p => p.Inventory)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (productExists is null) return NotFound("Product was not found or Deleted by the Admin");

        var result = new ProductResponseDto
        {
            productId = id,
            productName = productExists.Name,
            productDescription = productExists.Description,
            price = productExists.Price,
            quantity = productExists.Inventory.StockQuantity,
            category = productExists.Category.CategoryName

        };

        return Ok(result);
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int? id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        _context.Entry(product).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
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

    // POST: api/Product
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductCreationDto dto)
    {
        var newProduct = new Product
        {
            Name = dto.productName,
            Description = dto.productDescription,
            Price = dto.price,
            CategoryId = dto.categoryId,
            Inventory = new Inventory
            {
                StockQuantity = dto.quantity
            }
        };

        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();

        return Ok("Product Created");
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int? id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int? id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
