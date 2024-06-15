using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Filtering;
using BusinessLogic.Services.Repositories;
using BusinessLogic.Validators.Category;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using BusinessLogic.ViewModels.General;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Result<CategoryViewModel>> CreateCategoryAsync(CategoryCreateModel model)
        {
            var validator = new CategoryCreateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var category = _mapper.Map<Category>(model);
            await _categoryRepository.AddAsync(category);

            var confirmationResult = await _categoryRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok(_mapper.Map<CategoryViewModel>(category))
                : Result.Fail("Failed to add category");
        }

        public async Task<Result> UpdateCategoryAsync(CategoryUpdateModel model)
        {
            var validator = new CategoryUpdateValidator();
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            var category = await _categoryRepository.GetAsync(model.Id);
            if (category == null)
            {
                return Result.Fail("Unable to find this category");
            }

            category.Name = model.Name;
            category.Description = model.Description;

            var confirmationResult = await _categoryRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok()
                : Result.Fail("Failed to update category");
        }

        public async Task<Result> DeleteCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(categoryId);
            if (category is null)
            {
                return Result.Fail("Unable to find this category");
            }

            _categoryRepository.Remove(category);

            var confirmationResult = await _categoryRepository.ConfirmAsync();
            return confirmationResult > 0
                ? Result.Ok()
                : Result.Fail("Failed to delete category");
        }

        public async Task<Result<CategoryViewModel>> GetCategoryAsync(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(categoryId);
            if (category is null)
            {
                return Result.Fail("Unable to find this category");
            }
            var categoryView = _mapper.Map<CategoryViewModel>(category);
            return categoryView;
        }

        public async Task<Result<IEnumerable<CategoryViewModel>>> GetCategories()
        {
            IEnumerable<Category> categories;
            categories = await _categoryRepository.GetAllAsync(null);
            var response = _mapper.Map<IEnumerable<CategoryViewModel>>(categories);

            return Result.Ok(response);
        }
    }
}
