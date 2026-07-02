using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TO_DO_List.Models;
using TO_DO_List.Services.CategoryService;
using TO_DO_List.Services.TaskServise;
using Azure;

namespace TO_DO_List.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITaskService _taskService;

        public CategoriesApiController(ICategoryService categoryService, ITaskService taskService)
        {
            _categoryService = categoryService;
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Category>>> GetCategories([FromQuery] int page = 1 , [FromQuery] int pageSize = 16)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var categories = await _categoryService.GetCategories(userId,page, pageSize);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var category = await _categoryService.GetCategoryById(id, userId);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var category = new Category { Name = request.Name, UserId = userId };
            await _categoryService.CreateCategory(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Category name is required");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var category = new Category { Id = id, Name = request.Name, UserId = userId };
            await _categoryService.EditCategory(category);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");
            var category = await _categoryService.GetCategoryById(id, userId);
            if (category == null)
            {
                // Якщо він її не бачить, він поверне 404, і Angular покаже червону помилку!
                return NotFound($"Категорію з ID {id} для юзера {userId} не знайдено в БД.");
            }
            // Debug: виведемо в консоль (або в Response) те, що ми маємо
          
            await _categoryService.DeleteCategory(id, userId);
            return NoContent();
        }
    }

    public class CreateCategoryRequest
    {
        public string Name { get; set; }
    }
}
