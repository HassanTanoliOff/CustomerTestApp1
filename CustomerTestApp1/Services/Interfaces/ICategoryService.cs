using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ResultData<List<CategoriesResponseDto>>> GetAllCategoriesAsync();
        Task<ResultData<CategoriesResponseDto>> AddNewCategoryAsync(string categoryName);
        Task<ResultData<string>> UpdateCategoryAsync(int categoryId);
        Task<ResultData<string>> DeleteCategoryAsync(int categoryId);
    }
}
