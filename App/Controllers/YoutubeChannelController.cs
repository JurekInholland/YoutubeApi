using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Services.YoutubeService;

namespace App.Controllers;

/// <summary>
/// YoutubeChannelController
/// </summary>
public class YoutubeChannelController : BaseController
{
    private readonly ILogger<YoutubeChannelController> _logger;
    private readonly IYoutubeService _youtubeService;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// YoutubeChannelController constructor
    /// </summary>
    public YoutubeChannelController(ILogger<YoutubeChannelController> logger, IYoutubeService youtubeService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _youtubeService = youtubeService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get info about a youtube channel
    /// including a collection of videos
    /// </summary>
    [HttpGet("", Name = nameof(GetChannelInfo))]
    public async Task<IActionResult> GetChannelInfo(string channelId)
    {
        // var dbChannel = await _unitOfWork.YoutubeChannels.Where(x => x.Name == channelId).FirstOrDefaultAsync();
        var channel = await _youtubeService.GetChannelInfo(channelId);
        return Ok(channel);
    }

    [HttpGet("meta", Name = nameof(GetMetadata))]
    public async Task<IActionResult> GetMetadata(string channelId)
    {
        var channel = await _youtubeService.GetMetadata(channelId);
        return Ok(channel);
    }
}
