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
    /// Scrape a youtube channel for videos via id
    /// </summary>
    /// <remarks>Only the last ~40 videos are scraped</remarks>
    /// <param name="channelId">The id of the channel to scrape</param>
    /// <response code="200">List of videos</response>
    [HttpGet(nameof(YoutubeChannelById))]
    public async Task<IActionResult> YoutubeChannelById(string channelId)
    {
        try
        {
            var yt = await _scrapeService.ScrapeChannelById(channelId);
            Response.Headers.Add("Count", yt.Length.ToString());
            return Ok(yt);
        }

        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Scrape a youtube channel for videos via handle
    /// </summary>
    [HttpPost(nameof(YoutubeChannelByHandle))]
    public async Task<IActionResult> YoutubeChannelByHandle([FromBody] string handle)
    {
        _logger.LogInformation("Scraping channel {Handle}", handle);
        try
        {
            var yt = await _scrapeService.ScrapeChannelByHandle(handle);

            Response.Headers.Add("Count", yt.Length.ToString());
            return Ok(yt);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Scrape youtube search results for a given query
    /// </summary>
    [HttpGet(nameof(SearchResults))]
    public async Task<IActionResult> SearchResults(string query)
    {
        var res = await _scrapeService.ScrapeYoutubeSearchResults(query);
        Response.Headers.Add("Count", res.Length.ToString());

        return Ok(res);
    }


    /// <summary>
    /// Scrape multiple youtube videos from a list of urls
    /// </summary>
    [HttpPost(nameof(Multiple))]
    public async Task<IActionResult> Multiple([FromBody] string[] videoIds)
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
    [HttpGet(nameof(YoutubeVideo))]
    public async Task<IActionResult> YoutubeVideo(string videoId)
    {
        YoutubeVideo? yt;
        try
        {
            yt = await _scrapeService.ScrapeYoutubeVideo(videoId);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        if (yt is null)
        {
            return NotFound();
        }

        await _unitOfWork.YoutubeVideos.Create(yt);

        return Ok(yt);
    }

    /// <summary>
    /// Return raw html from a url to debug custom parsers
    /// </summary>
    [HttpGet(nameof(RawHtml))]
    public async Task<IActionResult> RawHtml(string url)
    {
        var html = await _scrapeService.GetRawHtml(url);
        return Ok(html);
    }
}
