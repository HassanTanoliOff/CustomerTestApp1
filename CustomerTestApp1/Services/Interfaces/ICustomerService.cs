using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ResultData<CustomerResponseDto>> GetCustomerByIdAsync(int cId);
        Task<ResultData<List<CustomerResponseDto>>> GetAllCustomersAsync();
        Task<ResultData<CustomerResponseDto>> AddCustomerAsync(CustomerCreationDto dto);
        Task<ResultData<CustomerResponseDto>> UpdateCustomerAsync(int id, CustomerUpdateDto dto);
        Task<ResultData<bool>> DeleteCustomerAsync(int id);

    }
}
