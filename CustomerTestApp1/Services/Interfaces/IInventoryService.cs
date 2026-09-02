using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<ResultData<string>> GetInventoryAsync();
        Task<ResultData<string>> GetInventoryByFilters(string condition);
    }
}
