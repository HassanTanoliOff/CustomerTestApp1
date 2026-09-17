using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces;

public interface IInventoryService {
  Task<ResultData<List<InventoryResponseDto>>> GetInventoryAsync();
  //Task<ResultData<IQueryable<InventoryResponseDto>>> GetInventoryByFilters(string condition);
}