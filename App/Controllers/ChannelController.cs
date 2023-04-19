using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>
/// Controller for managing channels
/// </summary>
public class ChannelController : BaseController
{
    private readonly ILogger<ChannelController> _logger;

    /// <summary>
    /// ChannelController constructor
    /// </summary>
    public ChannelController(ILogger<ChannelController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get subscriptions
    /// </summary>
    /// <returns></returns>
    [HttpGet(nameof(Subscriptions))]
    public async Task<IActionResult> Subscriptions()
    {
        return Ok();
    }
}
