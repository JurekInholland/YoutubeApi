using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;
using Services.YoutubeExplodeService;
using YoutubeExplode.Exceptions;

namespace App.Controllers;

/// <summary>
/// Controller for youtube video information
/// </summary>
public class YoutubeVideoController : BaseController
{
    private readonly ILogger<YoutubeVideoController> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYoutubeExplodeService _youtubeExplodeService;

    /// <summary>
    /// YoutubeVideoController constructor
    /// </summary>
    public YoutubeVideoController(ILogger<YoutubeVideoController> logger, IUnitOfWork unitOfWork,
        IYoutubeExplodeService youtubeExplodeService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _youtubeExplodeService = youtubeExplodeService;
    }

    /// <summary>
    /// Get a youtube video
    /// </summary>
    [HttpGet("", Name = nameof(GetYoutubeVideo))]
    public async Task<IActionResult> GetYoutubeVideo(string videoId)
    {
        _logger.LogInformation("Getting video {VideoId}", videoId);

        YoutubeVideo? video = await _unitOfWork.YoutubeVideos.Where(x => x.Id == videoId).FirstOrDefaultAsync();
        if (video is not null) return Ok(video);

        _logger.LogInformation("Video {VideoId} not found in database, getting from youtube", videoId);
        try
        {
            video = await _youtubeExplodeService.GetVideo(videoId);
        }
        catch (VideoUnavailableException e)
        {
            _logger.LogInformation("Video with id {VideoId} is unavailable", videoId);
            return NotFound(e.Message);
        }

        return Ok(video);
    }
}
