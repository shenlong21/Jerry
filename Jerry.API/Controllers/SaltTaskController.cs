using Jerry.API.Repositories.Interfaces;
using Jerry.API.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.ComponentModel;


namespace Jerry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaltTaskController : ControllerBase
    {
        private readonly ISaltTaskRepository _taskRepository;
        private readonly ILogger<SaltTaskController> _logger;


        public SaltTaskController(ISaltTaskRepository taskRepository, ILogger<SaltTaskController> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
            if (task is null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskRequestModel task)
        {
            var newTask = await _taskRepository.CreateTaskAsync(task);
            // return CreatedAtAction(nameof(GetTaskByIdAsync), new { id = newTask.Id }, newTask);
            return Ok(newTask);
        }

        [HttpPost("TaskUpdateForOneUser")]
        public async Task<IActionResult> TaskUpdateForOneUser([FromBody] TaskUpdateForOneUserRequestModel request)
        {
            var update = await _taskRepository.TaskUpdateForOneUser(request);

            if (!update)
            {
                return NotFound();
            }
            return Ok(update);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> UpdateTaskAsync(int id, [FromBody] UpdateTaskRequestModel task)
        // {
        //     var updatedTask = await _taskRepository.UpdateTaskAsync(id, task);
        //     if (updatedTask is null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(updatedTask);
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteTaskAsync(int id)
        // {
        //     var deleted = await _taskRepository.DeleteTaskAsync(id);
        //     if (!deleted)
        //     {
        //         return NotFound();
        //     }
        //     return NoContent();
        // }
    }
}
