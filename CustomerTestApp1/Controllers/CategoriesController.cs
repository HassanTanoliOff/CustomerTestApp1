
using CustomerTestApp1.Data;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Category
    [HttpGet]
    public Task GetCategory()
    {
        return Task.CompletedTask;
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public Task GetCategory(int id)
    {
        return Task.CompletedTask;
    }

    // PUT: api/Category/5
    [HttpPut("{id}")]
    public Task PutCategory(int? id)
    {
        return Task.CompletedTask;
    }

    // POST: api/Category
    [HttpPost]
    public Task PostCategory()
    {


        return Task.CompletedTask;
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public Task DeleteCategory(int? id)
    {
        return Task.CompletedTask;
    }

    private Task CategoryExists(int? id)
    {
        return Task.CompletedTask;
    }
}
