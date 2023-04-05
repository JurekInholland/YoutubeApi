using Microsoft.AspNetCore.Mvc;
using Services.TaskService;

namespace App.Controllers;

/// <summary>
/// Handle task scheduling
/// </summary>
public class TaskController : BaseController
{
    private readonly ILogger<TaskController> _logger;
    private readonly ITaskService _taskService;


    /// <summary>
    /// TaskController constructor
    /// </summary>
    public TaskController(ILogger<TaskController> logger, ITaskService taskService)
    {
        _logger = logger;
        _taskService = taskService;
    }


    /// <summary>
    ///    Set the interval for the task
    /// </summary>
    [HttpGet("interval", Name = nameof(SetTaskInterval))]
    public IActionResult SetTaskInterval(int interval)
    {
        TimeSpan intervalTimeSpan = TimeSpan.FromMilliseconds(interval);
        _taskService.ChangeInterval(intervalTimeSpan);
        return Ok();
    }
}
