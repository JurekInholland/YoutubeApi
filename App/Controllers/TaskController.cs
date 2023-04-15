using Microsoft.AspNetCore.Mvc;
using Services.TaskService;

namespace App.Controllers;

/// <summary>
/// Handle background task scheduling
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
    [HttpGet(nameof(SetTaskInterval))]
    public IActionResult SetTaskInterval(int interval)
    {
        _logger.LogInformation("Setting task interval to {Interval}", interval);
        TimeSpan intervalTimeSpan = TimeSpan.FromMilliseconds(interval);
        _taskService.ChangeInterval(intervalTimeSpan);
        return Ok();
    }


    /// <summary>
    ///   Set the interval for the channel scan task
    /// </summary>
    /// <param name="intervalSeconds">The channel scan interval in seconds</param>
    /// <returns></returns>
    [HttpPut(nameof(SetChannelScanInterval))]
    public IActionResult SetChannelScanInterval([FromBody] int intervalSeconds)
    {
        _logger.LogInformation("Setting channel scan interval to {Interval} seconds", intervalSeconds);
        TimeSpan intervalTimeSpan = TimeSpan.FromSeconds(intervalSeconds);

        _taskService.ChangeInterval(intervalTimeSpan);
        return Ok();
    }
}
