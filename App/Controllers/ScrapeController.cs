using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.DomainModels;
using Services.ScrapeService;

namespace App.Controllers;

/// <summary>
/// ScrapeController
/// </summary>
public class ScrapeController : BaseController
{
    private readonly IScrapeService _scrapeService;
    private readonly ILogger<ScrapeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// ScrapeController constructor
    /// </summary>
    public ScrapeController(ILogger<ScrapeController> logger, IScrapeService scrapeService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _scrapeService = scrapeService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Scrape multiple youtube videos from a list of urls
    /// </summary>
    [HttpPost("multiple", Name = nameof(ScrapeMultiple))]
    public async Task<IActionResult> ScrapeMultiple(string[] ids)
    {
        Task<YoutubeVideo>[] tasks = new Task<YoutubeVideo>[ids.Length];
        for (int i = 0; i < ids.Length; i++)
        {
            tasks[i] = _scrapeService.ScrapeYoutubeVideo(ids[i]);
        }

        await Task.WhenAll(tasks);
        return Ok(tasks);
    }

    /// <summary>
    /// Scrape a youtube video from a url via regex patterns
    /// </summary>
    [HttpGet("video", Name = nameof(ScrapeYoutubeVideo))]
    public async Task<IActionResult> ScrapeYoutubeVideo(string videoId)
    {
        var yt = await _scrapeService.ScrapeYoutubeVideo(videoId);

        await _unitOfWork.YoutubeVideos.Create(yt);

        return Ok(yt);
    }

    /// <summary>
    /// Return raw html from a url to debug custom parsers
    /// </summary>
    [HttpGet("html", Name = nameof(ScrapeRawHtml))]
    public async Task<IActionResult> ScrapeRawHtml(string url)
    {
        var html = await _scrapeService.GetRawHtml(url);
        return Ok(html);
    }
}
