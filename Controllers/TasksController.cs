using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Hubs;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly IHubContext<TaskHub> _hubContext;

    public TasksController(ITaskRepository taskRepository, IHubContext<TaskHub> hubContext)
    {
        _taskRepository = taskRepository;
        _hubContext = hubContext;
    }

    // GET: api/tasks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllAsync();
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate,
            CreatedByUserId = t.CreatedByUserId,
            CreatedByUsername = t.CreatedBy?.Username ?? "Unknown"
        });

        return Ok(taskDtos);
    }

    // GET: api/tasks/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound(new { message = "Task not found" });

        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            DueDate = task.DueDate,
            CreatedByUserId = task.CreatedByUserId,
            CreatedByUsername = task.CreatedBy?.Username ?? "Unknown"
        };

        return Ok(taskDto);
    }

    // GET: api/tasks/my-tasks
    [HttpGet("my-tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token" });

        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate,
            CreatedByUserId = t.CreatedByUserId,
            CreatedByUsername = t.CreatedBy?.Username ?? "Unknown"
        });

        return Ok(taskDtos);
    }

    // GET: api/tasks/user/{userId}
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByUserId(Guid userId)
    {
        var tasks = await _taskRepository.GetTasksByUserIdAsync(userId);
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate,
            CreatedByUserId = t.CreatedByUserId,
            CreatedByUsername = t.CreatedBy?.Username ?? "Unknown"
        });

        return Ok(taskDtos);
    }

    // GET: api/tasks/status/{status}
    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByStatus(int status)
    {
        if (!Enum.IsDefined(typeof(Domain.Entities.TaskStatus), status))
            return BadRequest(new { message = "Invalid status value" });

        var taskStatus = (Domain.Entities.TaskStatus)status;
        var tasks = await _taskRepository.GetTasksByStatusAsync(taskStatus);
        var taskDtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status.ToString(),
            Priority = t.Priority.ToString(),
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DueDate = t.DueDate,
            CreatedByUserId = t.CreatedByUserId,
            CreatedByUsername = t.CreatedBy?.Username ?? "Unknown"
        });

        return Ok(taskDtos);
    }

    // POST: api/tasks
    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token" });

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = createDto.Title,
            Description = createDto.Description,
            Priority = (TaskPriority)createDto.Priority,
            DueDate = createDto.DueDate,
            CreatedByUserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskRepository.AddAsync(task);

        var taskDto = new TaskDto
        {
            Id = createdTask.Id,
            Title = createdTask.Title,
            Description = createdTask.Description,
            Status = createdTask.Status.ToString(),
            Priority = createdTask.Priority.ToString(),
            CreatedAt = createdTask.CreatedAt,
            UpdatedAt = createdTask.UpdatedAt,
            DueDate = createdTask.DueDate,
            CreatedByUserId = createdTask.CreatedByUserId
        };

        // 🔥 NOTIFICATION EN TEMPS RÉEL - Tous les clients connectés reçoivent cette notification!
        await _hubContext.Clients.All.SendAsync("TaskCreated", taskDto);

        return CreatedAtAction(nameof(GetTaskById), new { id = taskDto.Id }, taskDto);
    }

    // PUT: api/tasks/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateDto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound(new { message = "Task not found" });

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token" });

        if (task.CreatedByUserId != userId)
            return Forbid();

        if (!string.IsNullOrEmpty(updateDto.Title))
            task.Title = updateDto.Title;

        if (updateDto.Description != null)
            task.Description = updateDto.Description;

        if (updateDto.Status.HasValue && Enum.IsDefined(typeof(Domain.Entities.TaskStatus), updateDto.Status.Value))
            task.Status = (Domain.Entities.TaskStatus)updateDto.Status.Value;

        if (updateDto.Priority.HasValue && Enum.IsDefined(typeof(TaskPriority), updateDto.Priority.Value))
            task.Priority = (TaskPriority)updateDto.Priority.Value;

        if (updateDto.DueDate.HasValue)
            task.DueDate = updateDto.DueDate;

        task.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);

        var taskDto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            Priority = task.Priority.ToString(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            DueDate = task.DueDate,
            CreatedByUserId = task.CreatedByUserId
        };

        // 🔥 NOTIFICATION EN TEMPS RÉEL - Tous les clients savent que la tâche a été modifiée!
        await _hubContext.Clients.All.SendAsync("TaskUpdated", taskDto);

        return NoContent();
    }

    // DELETE: api/tasks/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
            return NotFound(new { message = "Task not found" });

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized(new { message = "Invalid token" });

        if (task.CreatedByUserId != userId)
            return Forbid();

        await _taskRepository.DeleteAsync(id);

        // 🔥 NOTIFICATION EN TEMPS RÉEL - Tous les clients savent que la tâche a été supprimée!
        await _hubContext.Clients.All.SendAsync("TaskDeleted", id);

        return NoContent();
    }
}

//** Flow temps réel:**
 
//User A crée une tâche → API → SignalR Hub → TOUS les clients(A, B, C...)
//reçoivent la notification instantanément! ⚡