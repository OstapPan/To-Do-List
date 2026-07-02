using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TO_DO_List.Models;
using TO_DO_List.Services.TaskServise;

namespace TO_DO_List.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksApiController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TaskVeiwModel>>> GetTasks([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var tasks = await _taskService.GetTasks(userId, page, pageSize);
            if (tasks == null)
                return NotFound();

            var models = new List<TaskVeiwModel>();
            foreach (var task in tasks.Items)
            {
                if (task is TaskRepetitiveAcions repTask)
                    models.Add(new TaskVeiwModel(repTask));
                else
                    models.Add(new TaskVeiwModel((TaskToDo)task));
            }
            return Ok(new { Items = models, TotalCount = tasks.TotalCount });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskVeiwModel>> GetTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            var task = await _taskService.GetTasksById(id, userId);
            if (task == null)
                return NotFound();

            TaskVeiwModel model = new TaskVeiwModel(task);
            return Ok(model);
        }


        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TaskVeiwModel>>> GetTasksByCategory(int categoryId)
        {
        var tasks = await _taskService.GetTasksByCategoryIdAsync(categoryId , User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (tasks == null)
        {
            return NotFound("Категорію не знайдено або завдань немає.");
        }

        return Ok(tasks);
    }

        [HttpPost]
        public async Task<ActionResult<TaskVeiwModel>> CreateTask([FromBody] TaskVeiwModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            model.UserID = userId;

            ITaskToDo task;
            if (model.IsRepetitive == false)
            {
                task = new TaskToDo(model);
            }
            else
            {
                model.HowOften = model.HowOften ?? 1;
                task = new TaskRepetitiveAcions(model);
            }

            task.Category = null;
            await _taskService.CreateTask(task);

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, new TaskVeiwModel(task));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskVeiwModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            model.Id = id;
            model.UserID = userId;

            ITaskToDo task;
            if (model.IsRepetitive == false)
            {
                task = new TaskToDo(model);
            }
            else
            {
                task = new TaskRepetitiveAcions(model);
            }

            await _taskService.EditTask(task);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("User ID not found");

            await _taskService.DeleteTask(id, userId);
            return NoContent();
        }
    [HttpGet("search/{searchtext}")]
    public async Task<ActionResult<IEnumerable<TaskVeiwModel>>> SearchTasksByText( string searchtext)
    {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (userId == null)
        return BadRequest("User ID not found");

    if (string.IsNullOrWhiteSpace(searchtext))
    {
        return Ok(new List<TaskVeiwModel>());
    }

    var tasks = await _taskService.SearchTasksByTextAsync(searchtext, userId);
    
    if (tasks == null || !tasks.Any())
    {
        return Ok(new List<TaskVeiwModel>()); 
    }

    return Ok(tasks);
}
    }
}
