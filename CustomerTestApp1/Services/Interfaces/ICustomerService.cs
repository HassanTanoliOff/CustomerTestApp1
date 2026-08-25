using CustomerTestApp1.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ActionResult<IEnumerable<CustomerResponseDto>>> GetAllCustomersAsync();
        Task<ActionResult<CustomerResponseDto>> AddCustomerAsync(CustomerCreationDto dto);
        Task<ActionResult<CustomerUpdateDto>> UpdateCustomerAsync(int id, CustomerUpdateDto dto);
        Task<ActionResult<bool>> DeleteCustomerAsync(int id);
    }
}
