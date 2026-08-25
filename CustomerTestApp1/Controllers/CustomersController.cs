using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAll()
    {
        var customers = await _context.Customers
            .Select(c => new CustomerResponseDto
            {
                CustomerId = c.CostumerId,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
            })
            .ToListAsync();

        if (!customers.Any()) return NotFound();

        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> AddCustomer(CustomerCreationDto customer)
    {
        var newCustomer = new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Address = customer.Address,
        };

        _context.Customers.Add(newCustomer);
        await _context.SaveChangesAsync();
        //return Ok(newCustomer);

        var result = new CustomerResponseDto
        {
            CustomerId = newCustomer.CostumerId,
            FirstName = newCustomer.FirstName,
            LastName = newCustomer.LastName,
            PhoneNumber = newCustomer.PhoneNumber,
            Email = newCustomer.Email,
            Address = newCustomer.Address,
        };
        return Ok(result);
    }

    [HttpPatch]
    public async Task<ActionResult<CustomerUpdateDto>> UpdateCustomer(int id, CustomerUpdateDto customerDto)
    {

        var isExisting = await _context.Customers.FirstOrDefaultAsync(c => c.CostumerId == id);
        if (isExisting == null) return NotFound($"The customer with Id:{id} Not found.");

        string firstName = isExisting.FirstName = string.IsNullOrWhiteSpace(customerDto.FirstName) ? isExisting.FirstName : customerDto.FirstName;
        string lastName = isExisting.LastName = string.IsNullOrWhiteSpace(customerDto.LastName) ? isExisting.LastName : customerDto.LastName;
        string phoneNumber = isExisting.PhoneNumber = string.IsNullOrWhiteSpace(customerDto.PhoneNumber) ? isExisting.PhoneNumber : customerDto.PhoneNumber;
        string address = isExisting.Address = string.IsNullOrWhiteSpace(customerDto.Address) ? isExisting.Address : customerDto.Address;



        await _context.SaveChangesAsync();


        var updatedInfo = new CustomerUpdateDto
        {
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
        };
        return Ok(updatedInfo);

    }

    [HttpDelete]
    public async Task<ActionResult<String>> DeleteCustomer(int id)
    {
        int ifExists = await _context.Customers
            .Where(c => c.CostumerId == id)
            .ExecuteDeleteAsync();

        if (ifExists == 0) return NotFound("Customer does not exist or incorrect Id input.");

        return Ok("Customer Deleted");
    }
}

