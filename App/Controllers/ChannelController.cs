using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

/// <summary>
/// Controller for managing channels
/// </summary>
public class ChannelController : BaseController
{
    private readonly ILogger<ChannelController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// ChannelController constructor
    /// </summary>
    public ChannelController(ILogger<ChannelController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get subscriptions
    /// </summary>
    /// <returns></returns>
    [HttpGet(nameof(Subscriptions))]
    public async Task<IActionResult> Subscriptions()
    {
        _logger.LogInformation("Getting subscriptions");
        var subs = await _unitOfWork.SubscribedChannels.All().ToListAsync();
        return Ok(subs);
    }
}
