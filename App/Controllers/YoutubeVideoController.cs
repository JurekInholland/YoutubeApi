using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;
using Services.ScrapeService;
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
    private readonly IScrapeService _scrapeService;

    /// <summary>
    /// YoutubeVideoController constructor
    /// </summary>
    public YoutubeVideoController(ILogger<YoutubeVideoController> logger, IUnitOfWork unitOfWork,
        IYoutubeExplodeService youtubeExplodeService, IScrapeService scrapeService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _youtubeExplodeService = youtubeExplodeService;
        _scrapeService = scrapeService;
    }

    /// <summary>
    /// Retrieve a youtube video by id from database. If not found, scrape youtube for video information
    /// </summary>
    [HttpGet(nameof(GetVideo))]
    public async Task<IActionResult> GetVideo(string videoId)
    {
        YoutubeVideo? video = await _unitOfWork.YoutubeVideos.Where(x => x.Id == videoId).Include(y => y.LocalVideo)
            .Include(x => x.YoutubeChannel).FirstOrDefaultAsync();
        if (video is not null) return Ok(video);

        try
        {
            video = await _scrapeService.ScrapeYoutubeVideo(videoId);
        }
        catch (Exception e)
        {
            _logger.LogWarning("EXCEPTION DURING SCRAPE: {Exception}", e);
        }

        return Ok(video);
    }


    /// <summary>
    /// Get all locally available youtube videos
    /// </summary>
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all videos");
        var videos = await _unitOfWork.YoutubeVideos.All()
            .Include(x => x.LocalVideo)
            .Include(x => x.YoutubeChannel)
            .Where(y => y.LocalVideo != null).ToListAsync();
        return Ok(videos);
    }
}
