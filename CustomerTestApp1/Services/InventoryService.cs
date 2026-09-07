using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Logs;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;
        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultData<List<InventoryResponseDto>>> GetInventoryAsync()
        {
            try
            {

                var inventory = await _context.Inventory
                        .AsNoTracking()
                        .Select(i => new InventoryResponseDto
                        {
                            Productid = i.ProductId,
                            ProductName = i.Product.Name,
                            StockQuantity = i.StockQuantity,
                            Category = i.Product.Category.CategoryName,
                            DateCreated = i.CreatedAt,
                            DateUpdated = i.UpdatedAt
                        })
                        .ToListAsync();
                if (inventory.Count == 0)
                    return ResultData<List<InventoryResponseDto>>.Pass(inventory, "Inventory is Empty.");

                return ResultData<List<InventoryResponseDto>>.Pass(inventory, "All Items Retrieved");
            }
            catch (Exception ex)
            {
                CatchExceptionConsoleLog.Debug(ex);
                return ResultData<List<InventoryResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
            }
        }

        //    public async Task<ResultData<IQueryable<InventoryResponseDto>>> GetInventoryByFilters(string condition)
        //    {

        //        if (string.IsNullOrWhiteSpace(condition))
        //            return ResultData<IQueryable<InventoryResponseDto>>.Fail("Condition is required to search with a filter.", ResultErrorType.Validation);

        //        /// group by 

        //        try
        //        {

        //            // IQueryable<Inventory>
        //            var inventory = _context.Inventory;





        //        }
        //        catch (Exception ex)
        //        {
        //            CatchExceptionConsoleLog.Debug(ex);
        //            return ResultData<IQueryable<InventoryResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
        //        }
        //    }
    }
}
