using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Responses;
using CustomerTestApp1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerTestApp1.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ResultData<List<CategoriesResponseDto>>> GetAllCategoriesAsync()
        {
            var cats = await _context.Categories.ToListAsync();



            return ResultData<List<CategoriesResponseDto>>.Fail("something went wrong", ResultErrorType.Unknown);

        }

        public Task<ResultData<CategoriesResponseDto>> AddNewCategoryAsync(string categoryName)
        {
            throw new NotImplementedException();
        }



        public Task<ResultData<string>> UpdateCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }


        public Task<ResultData<string>> DeleteCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }
    }
}