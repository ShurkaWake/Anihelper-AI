using API.Extensions;
using API.Requests.Category;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Core;
using BusinessLogic.Filtering;
using BusinessLogic.Services;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/category")]
    [Authorize]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetCategories();
            return categories.ToObjectResponse();
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryAsync([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            return category.ToObjectResponse();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryCreateModel model)
        {
            var creationResult = await _categoryService.CreateCategoryAsync(model);
            if (creationResult.IsFailed)
            {
                return creationResult.ToObjectResponse();
            }

            return CreatedAtAction("CreateCategory", creationResult.Value);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> UpdateCategoryAsync([FromRoute] int id, [FromBody] CategoryUpdateRequest request)
        {
            var categoryModel = _mapper.Map<CategoryUpdateModel>(request);
            categoryModel.Id = id;
            var updateResult = await _categoryService.UpdateCategoryAsync(categoryModel);
            return updateResult.ToNoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteCategoryAsync([FromRoute] int id)
        {
            var deletionResult = await _categoryService.DeleteCategoryAsync(id);
            return deletionResult.ToNoContent();
        }
    }
}
