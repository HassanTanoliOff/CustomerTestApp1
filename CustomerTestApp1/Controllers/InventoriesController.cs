using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public InventoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Inventory
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryResponseDto>>> GetInventory()
    {
        var inventoryResult = await _context.Inventory
             .Select(i => new InventoryResponseDto
             {
                 Id = i.Id,
                 Productid = i.ProductId,
                 ProductName = i.Product.Name,
                 StockQuantity = i.StockQuantity,
                 DateCreated = i.CreatedAt,
             }).ToListAsync();

        return Ok(inventoryResult);


    }

    // GET: api/Inventory/5
    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryResponseDto>> GetInventory(int id)
    {
        var isExists = await _context.Inventory
            .Where(i => i.Id == id)
            .Select(i => new InventoryResponseDto
            {
                Id = i.Id,
                Productid = i.ProductId,
                ProductName = i.Product.Name,
                StockQuantity = i.StockQuantity,
                DateCreated = i.CreatedAt,
            }).AsNoTracking().ToListAsync();

        if (isExists == null || !isExists.Any())
        {
            return NotFound("Inventory item does not exist or was deleted.");
        }
        return Ok(isExists);
    }
}
//    // PUT: api/Inventory/5
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPut("{id}")]
//    public async Task<IActionResult> PutInventory(int? id, Inventory inventory)
//    {
//        if (id != inventory.Id)
//        {
//            return BadRequest();
//        }

//        _context.Entry(inventory).State = EntityState.Modified;

//        try
//        {
//            await _context.SaveChangesAsync();
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!InventoryExists(id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }

//        return NoContent();
//    }

//    // POST: api/Inventory
//    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//    [HttpPost]
//    public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
//    {
//        _context.Inventory.Add(inventory);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction("GetInventory", new { id = inventory.Id }, inventory);
//    }

//    // DELETE: api/Inventory/5
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeleteInventory(int? id)
//    {
//        var inventory = await _context.Inventory.FindAsync(id);
//        if (inventory == null)
//        {
//            return NotFound();
//        }

//        _context.Inventory.Remove(inventory);
//        await _context.SaveChangesAsync();

//        return NoContent();
//    }

//    private bool InventoryExists(int? id)
//    {
//        return _context.Inventory.Any(e => e.Id == id);
//    }
//}
