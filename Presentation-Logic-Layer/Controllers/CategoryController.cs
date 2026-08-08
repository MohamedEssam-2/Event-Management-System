using Business_Logic_Layer.DTO.CategoryDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation_Logic_Layer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService _categoryService) :ControllerBase
    {
        
        [HttpGet]
        [AllowAnonymous]
        public async Task <IActionResult> GetAllCategories([FromQuery] string? Search, [FromQuery] int PageIndex = 1, [FromQuery] int PageSize = 5, [FromQuery] string? sortBy = null)
        {
            var categories =await _categoryService.GetAllCategories(Search, PageIndex, PageSize, sortBy);
            return Ok(categories);
        }
        [HttpGet("{categoryId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            return Ok(category);
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task <IActionResult> CreateCategory([FromForm] CategoryDTO categoryDTO)
        {
            var createdCategory = await _categoryService.CreateCategory(categoryDTO);
            return Ok(createdCategory);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var deleted = await _categoryService.DeleteCategory(categoryId);
            return Ok(deleted);
        }
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromForm] UpdateCategoryDTO categoryDTO)
        {
            var updated = await _categoryService.UpdateCat(categoryId, categoryDTO);
            return Ok(updated);
        }
    }
}
