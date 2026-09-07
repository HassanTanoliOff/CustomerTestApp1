using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Models;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services
{
    public class CustomerService(ApplicationDbContext context) : ICustomerService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<ResultData<CustomerResponseDto>> GetCustomerByIdAsync(int cId)
        {
            try
            {
                CustomerResponseDto? customer = await _context.Customers
                        .AsNoTracking()
                        .Where(c => c.CostumerId == cId)
                        .Where(c => c.IsDeleted == false)
                        .Select(c => new CustomerResponseDto
                        {
                            CustomerId = c.CostumerId,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            PhoneNumber = c.PhoneNumber,
                            Email = c.Email,
                            Address = c.Address,
                        })
                        .FirstOrDefaultAsync();

                if (customer == null) return
                        ResultData<CustomerResponseDto>.Fail($"Customer with Id: {cId} not found or was deleted", ResultErrorType.NotFound);

                return ResultData<CustomerResponseDto>.Pass(customer, "Customer Retrieved Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [Debug] Type: {ex.GetType().Name},Message: {ex.Message}, Inner: {ex.InnerException?.Message}.");
                return ResultData<CustomerResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
            }

        }
        public async Task<ResultData<List<CustomerResponseDto>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers
                    .AsNoTracking()
                    .Where(c => c.IsDeleted == false)
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
                if (customers.Count == 0)
                    return ResultData<List<CustomerResponseDto>>.Pass(customers, "No Customers were Found or Added yet.");

                return ResultData<List<CustomerResponseDto>>.Pass(customers, "Successfully retrieved all Customers");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Type {ex.GetType().Name} , Message: {ex.Message} , Inner: {ex.InnerException?.Message}");
                return ResultData<List<CustomerResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
            }
        }
        public async Task<ResultData<CustomerResponseDto>> AddCustomerAsync(CustomerCreationDto dto)
        {
            // First check customer is already created or not 

            var exists = await _context.Customers.AsNoTracking().AnyAsync(c => c.Email == dto.Email);
            if (exists)
                return ResultData<CustomerResponseDto>.Fail("User already exits.", ResultErrorType.Validation);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newCustomer = new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName ?? "",
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber ?? "",
                    Address = dto.Address ?? "No address added",
                    DateCreated = DateTime.UtcNow
                };

                await _context.Customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();

                var newUser = new User
                {

                };


                //}
                //catch (Exception ex)
                //{

                //}

                //var newCustomer = new Customer
                //{
                //    FirstName = dto.FirstName,
                //    LastName = dto.LastName,
                //    PhoneNumber = dto.PhoneNumber,
                //    Email = dto.Email,
                //    Address = dto.Address,
                //};
                //try
                //{

                //    _context.Customers.Add(newCustomer);
                //    await _context.SaveChangesAsync();

                //    var result = new CustomerResponseDto
                //    {
                //        CustomerId = newCustomer.CostumerId,
                //        FirstName = newCustomer.FirstName,
                //        LastName = newCustomer.LastName,
                //        PhoneNumber = newCustomer.PhoneNumber,
                //        Email = newCustomer.Email,
                //        Address = newCustomer.Address,
                //    };

                return ResultData<CustomerResponseDto>.Pass(new CustomerResponseDto(), "User Created");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [Debug] Type: {ex.GetType().Name} , Message:{ex.Message}, Inner: {ex.InnerException?.Message}.");
                return ResultData<CustomerResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
            }

        }
        public async Task<ResultData<CustomerResponseDto>> UpdateCustomerAsync(int id, CustomerUpdateDto dto)
        {
            var isExisting = await _context.Customers.FirstOrDefaultAsync(c => c.CostumerId == id);
            if (isExisting == null)
            {
                return ResultData<CustomerResponseDto>.Fail($"The customer with Id:{id} Not found.", ResultErrorType.NotFound);
            }

            string firstName = isExisting.FirstName = string.IsNullOrWhiteSpace(dto.FirstName)
                ? isExisting.FirstName : dto.FirstName;

            string lastName = isExisting.LastName = string.IsNullOrWhiteSpace(dto.LastName)
                ? isExisting.LastName : dto.LastName;

            string? phoneNumber = isExisting.PhoneNumber = string.IsNullOrWhiteSpace(dto.PhoneNumber)
                ? isExisting.PhoneNumber : dto.PhoneNumber;

            string address = isExisting.Address = string.IsNullOrWhiteSpace(dto.Address)
                ? isExisting.Address : dto.Address;


            try
            {

                await _context.SaveChangesAsync();


                var updatedInfo = new CustomerResponseDto
                {
                    CustomerId = isExisting.CostumerId,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = phoneNumber,
                    Address = address,
                };
                return ResultData<CustomerResponseDto>.Pass(updatedInfo, "User updated");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" [Debug] Type: {ex.GetType().Name} , Message:{ex.Message}, Inner: {ex.InnerException?.Message}.");
                return ResultData<CustomerResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
            }
        }
        public async Task<ResultData<bool>> DeleteCustomerAsync(int id)
        {
            Customer? isExisting = await _context.Customers.FirstOrDefaultAsync(c => c.CostumerId == id);

            if (isExisting == null)
            {
                return ResultData<bool>.Fail($"Customer with Id:{id} does not exists.", ResultErrorType.NotFound);
            }

            isExisting.IsDeleted = true;

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Debug] Type: {ex.GetType().Name} , Message: {ex.Message}, Inner: {ex.InnerException?.Message}");
                return ResultData<bool>.Fail(ex.Message, ResultErrorType.Unknown);
            }

            return ResultData<bool>.Pass(true, $"Customer with Id:{id} was deleted successfully");
        }

    }
}
