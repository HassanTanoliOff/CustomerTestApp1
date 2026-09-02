using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;

namespace CustomerTestApp1.Services
{
    public class InventoryService : IInventoryService
    {
        public Task<ResultData<string>> GetInventoryAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultData<string>> GetInventoryByFilters(string condition)
        {
            throw new NotImplementedException();
        }
    }
}
