﻿using Domain.Repositories;
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
    ///
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    [HttpGet(Name = nameof(GetYoutubeVideo))]
    public async Task<IActionResult> GetYoutubeVideo(string videoId)
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
    /// Get a youtube video DEPRECATED
    /// </summary>
    [HttpGet("old", Name = nameof(GetYoutubeVideoOld))]
    public async Task<IActionResult> GetYoutubeVideoOld(string videoId)
    {
        _logger.LogInformation("Getting video {VideoId}", videoId);

        YoutubeVideo? video = await _unitOfWork.YoutubeVideos.Where(x => x.Id == videoId).Include(y => y.LocalVideo).FirstOrDefaultAsync();
        if (video is not null) return Ok(video);

        _logger.LogInformation("Video {VideoId} not found in database, getting from youtube", videoId);
        try
        {
            video = await _youtubeExplodeService.GetVideo(videoId);
            Task.Run(() => _youtubeExplodeService.GetChannel(video.YoutubeChannel.Id));
        }
        catch (VideoUnavailableException e)
        {
            _logger.LogInformation("Video with id {VideoId} is unavailable", videoId);
            return NotFound(e.Message);
        }

        return Ok(video);
    }

    /// <summary>
    /// Get all locally available youtube videos
    /// </summary>
    [HttpGet("all", Name = nameof(GetAllYoutubeVideos))]
    public async Task<IActionResult> GetAllYoutubeVideos()
    {
        _logger.LogInformation("Getting all videos");
        var videos = await _unitOfWork.YoutubeVideos.All()
            .Include(x => x.LocalVideo)
            .Where(y => y.LocalVideo != null).ToListAsync();
        return Ok(videos);
    }
}
