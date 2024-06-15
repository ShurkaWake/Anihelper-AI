using BusinessLogic.Filtering;
using BusinessLogic.ViewModels.Category;
using BusinessLogic.ViewModels.General;
using FluentResults;

namespace BusinessLogic.Services
{
    public interface ICategoryService
    {
        Task<Result<CategoryViewModel>> CreateCategoryAsync(CategoryCreateModel model);
        Task<Result> DeleteCategoryAsync(int categoryId);
        Task<Result<IEnumerable<CategoryViewModel>>> GetCategories();
        Task<Result<CategoryViewModel>> GetCategoryAsync(int categoryId);
        Task<Result> UpdateCategoryAsync(CategoryUpdateModel model);
    }
}