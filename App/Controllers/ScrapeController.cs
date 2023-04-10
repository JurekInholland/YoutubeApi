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
    /// Scrape a youtube channel for videos
    /// </summary>
    [HttpGet("channel", Name = nameof(ScrapeYoutubeChannel))]
    public async Task<IActionResult> ScrapeYoutubeChannel(string channelId)
    {
        var yt = await _scrapeService.ScrapeYoutubeChannel(channelId);

        // await _unitOfWork.YoutubeChannels.Create(yt);

        Response.Headers.Add("Count", yt.Length.ToString());
        return Ok(yt);
    }

    /// <summary>
    /// Scrape youtube search results for a given query
    /// </summary>
    [HttpGet("search", Name = nameof(ScrapeSearchResults))]
    public async Task<IActionResult> ScrapeSearchResults(string query)
    {
        var res = await _scrapeService.ScrapeYoutubeSearchResults(query);
        return Ok(res);
    }


    /// <summary>
    /// Scrape multiple youtube videos from a list of urls
    /// </summary>
    [HttpPost("multiple", Name = nameof(ScrapeMultiple))]
    public async Task<IActionResult> ScrapeMultiple([FromBody] string[] videoIds)
    {
        _logger.LogInformation("Scraping {IdCount} videos", videoIds.Length);
        Task<YoutubeVideo?>[] tasks = new Task<YoutubeVideo?>[videoIds.Length];
        for (int i = 0; i < videoIds.Length; i++)
        {
            tasks[i] = _scrapeService.ScrapeYoutubeVideo(videoIds[i]);
        }

        var res = await Task.WhenAll(tasks);
        return Ok(res.Where(x => x != null).Select(x => x!).ToArray());
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
