using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<CustomerResponseDto>> AddCustomerAsync(CustomerCreationDto dto)
        {
            var newCustomer = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            var result = new CustomerResponseDto
            {
                CustomerId = newCustomer.CostumerId,
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName,
                PhoneNumber = newCustomer.PhoneNumber,
                Email = newCustomer.Email,
                Address = newCustomer.Address,
            };

            return result;
        }

        public Task<ActionResult<bool>> DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<CustomerUpdateDto>> UpdateCustomerAsync(int id, CustomerUpdateDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
