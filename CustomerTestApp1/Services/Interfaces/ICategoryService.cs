using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;

namespace CustomerTestApp1.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ResultData<List<CategoriesResponseDto>>> GetAllCategoriesAsync();
        Task<ResultData<CategoriesResponseDto>> AddNewCategoryAsync(CategoriesCreationDto dto);
        Task<ResultData<string>> UpdateCategoryAsync(int categoryId, CategoryUpdateDto dto);
        Task<ResultData<string>> DeleteCategoryAsync(int categoryId);
    }
}
