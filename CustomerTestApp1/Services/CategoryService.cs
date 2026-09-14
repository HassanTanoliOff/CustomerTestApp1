using CustomerTestApp1.Data;
using CustomerTestApp1.DTOS;
using CustomerTestApp1.Logs;
using CustomerTestApp1.Models;
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
      try
      {
        List<CategoriesResponseDto> cats = await _context
          .Categories.AsNoTracking()
          .Select(ct => new CategoriesResponseDto
          {
            CategoryId = ct.Id,
            CategoryName = ct.CategoryName,
            CategoryCreatedDate = ct.CreatedAt,
            CategoryModifiedDate = ct.UpdatedAt,
            Active = !ct.IsDeleted ? "Active" : "Deleted",
          })
          .OrderBy(ct => ct.CategoryName)
          .OrderBy(ct => ct.Active) // might not work as intended
          .ToListAsync();

        if (cats.Count == 0)
          return ResultData<List<CategoriesResponseDto>>.Pass(
            cats,
            "No Categories Created yet, or were deleted by Admin"
          );

        return ResultData<List<CategoriesResponseDto>>.Pass(
          cats,
          "All Categories Retrieved Successfully"
        );
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<List<CategoriesResponseDto>>.Fail(ex.Message, ResultErrorType.Unknown);
      }
    }

    public async Task<ResultData<CategoriesResponseDto>> AddNewCategoryAsync(
      CategoriesCreationDto dto
    )
    {
      var hasValues = !string.IsNullOrWhiteSpace(dto.CategoryName) && dto.CategoryName != "string";
      if (!hasValues)
        return ResultData<CategoriesResponseDto>.Fail(
          "Please Enter A Category Name ",
          ResultErrorType.Validation
        );

      string formattedCategoryNameFromDto = dto.CategoryName.Trim().ToLower();

      var catExists = await _context
        .Categories.AsNoTracking()
        .FirstOrDefaultAsync(ct => ct.CategoryName.ToLower() == formattedCategoryNameFromDto);

      if (catExists != null)
        return ResultData<CategoriesResponseDto>.Fail(
          $"Category :\"{dto.CategoryName}\" Already exists.",
          ResultErrorType.Conflict
        );

      var name = dto.CategoryName.Trim();
      var formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();

      var newCategory = new Category { CategoryName = formattedName };

      _context.Categories.Add(newCategory);

      try
      {
        await _context.SaveChangesAsync();

        var response = new CategoriesResponseDto
        {
          CategoryId = newCategory.Id,
          CategoryName = newCategory.CategoryName,
          CategoryCreatedDate = newCategory.CreatedAt,
          CategoryModifiedDate = newCategory.UpdatedAt,
          Active = !newCategory.IsDeleted ? "Active" : "Deleted",
        };

        return ResultData<CategoriesResponseDto>.Pass(response, "Category created.");
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<CategoriesResponseDto>.Fail(ex.Message, ResultErrorType.Unknown);
      }
    }

    public async Task<ResultData<string>> UpdateCategoryAsync(int categoryId, CategoryUpdateDto dto)
    {
      var isExisting = await _context.Categories.FirstOrDefaultAsync(ct =>
        ct.Id == categoryId && ct.IsDeleted == false
      );

      if (isExisting is null)
        return ResultData<string>.Fail(
          $"Category Id:{categoryId} does not Exists.",
          ResultErrorType.NotFound
        );

      if (string.IsNullOrWhiteSpace(dto.CategoryName) || dto.CategoryName == "string")
        return ResultData<string>.Pass("Enter a new Category name to update.", "Not updated.");

      if (
        string.Equals(
          isExisting.CategoryName,
          dto.CategoryName!.Trim(),
          StringComparison.OrdinalIgnoreCase
        )
      )
      {
        return ResultData<string>.Fail("Category Already Exists.", ResultErrorType.Conflict);
      }

      var name = dto.CategoryName.Trim();
      var formattedName = char.ToUpper(name[0]) + name.Substring(1).ToLower();

      var oldName = isExisting.CategoryName;
      isExisting.CategoryName = formattedName;
      isExisting.UpdatedAt = DateTime.Now;

      try
      {
        await _context.SaveChangesAsync();

        return ResultData<string>.Pass($"Category Changed From ", "");
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<string>.Fail(ex.Message, ResultErrorType.Unknown);
      }
    }

    public async Task<ResultData<string>> DeleteCategoryAsync(int categoryId)
    {
      var isExist = await _context.Categories.FirstOrDefaultAsync(ct =>
        ct.Id == categoryId && ct.IsDeleted == false
      );

      if (isExist is null)
        return ResultData<string>.Fail(
          $"The Category Id:{categoryId} Not Found.",
          ResultErrorType.NotFound
        );

      isExist.IsDeleted = true;

      try
      {
        await _context.SaveChangesAsync();

        return ResultData<string>.Pass(
          $"Category Id:{categoryId} , Name:{isExist.CategoryName} Was deleted.",
          " Deleted Successfully"
        );
      }
      catch (Exception ex)
      {
        CatchExceptionConsoleLog.Debug(ex);
        return ResultData<string>.Fail(ex.Message, ResultErrorType.Unknown);
      }
    }
  }
}
