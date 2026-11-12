
using Business_Logic_Layer;
using Business_Logic_Layer.DTO.CategoryDTO;
using Business_Logic_Layer.DTO.PaginationDTO;
using Business_Logic_Layer.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation_Logic_Layer.Attributes;

namespace Presentation_Logic_Layer.Controllers
{
    /// <summary>
    /// Handles category management operations, including retrieval,
    /// creation, updating, and deletion of categories.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController(ICategoryService _categoryService) : ControllerBase
    {
        /// <summary>
        /// Retrieves all categories with pagination, searching, and sorting support.
        /// </summary>
        /// <param name="Search">Optional search term used to filter categories.</param>
        /// <param name="PageIndex">The page number to retrieve. Defaults to 1.</param>
        /// <param name="PageSize">The number of categories per page. Defaults to 5.</param>
        /// <param name="sortBy">Optional field used to sort the categories.</param>
        /// <returns>A paginated list of categories matching the specified filters.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<PagedResultDTO<ReadCategoryDTO>>>> GetAllCategories(
            [FromQuery] string? Search,
            [FromQuery] int PageIndex = 1,
            [FromQuery] int PageSize = 5,
            [FromQuery] string? sortBy = null)
        {
            var categories = await _categoryService.GetAllCategories(Search, PageIndex, PageSize, sortBy!);
            return Ok(categories);
        }

        /// <summary>
        /// Retrieves detailed information about a specific category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>The details of the requested category.</returns>
        /// <remarks>
        /// This endpoint is publicly accessible.
        /// Results are cached using Redis.
        /// </remarks>
        [CacheRedis]
        [HttpGet("{categoryId:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<DetailsCategoryDTO>>> GetCategoryById(int categoryId)
        {
            var category = await _categoryService.GetById(categoryId);
            return Ok(category);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDTO">The category information, including any uploaded form data.</param>
        /// <returns>The ID of the newly created category.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// Accepts multipart/form-data.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<int>>> CreateCategory([FromForm] CategoryDTO categoryDTO)
        {
            var createdCategory = await _categoryService.CreateCategory(categoryDTO);
            return Ok(createdCategory);
        }

        /// <summary>
        /// Deletes an existing category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to delete.</param>
        /// <returns>True if the category was deleted successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// </remarks>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCategory(int categoryId)
        {
            var deleted = await _categoryService.DeleteCategory(categoryId);
            return Ok(deleted);
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category to update.</param>
        /// <param name="categoryDTO">The updated category information.</param>
        /// <returns>True if the category was updated successfully; otherwise, false.</returns>
        /// <remarks>
        /// Requires Admin role authorization.
        /// Accepts multipart/form-data.
        /// </remarks>
        [HttpPatch]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateCategory(
            int categoryId,
            [FromForm] UpdateCategoryDTO categoryDTO)
        {
            var updated = await _categoryService.UpdateCat(categoryId, categoryDTO);
            return Ok(updated);
        }
    }
}
