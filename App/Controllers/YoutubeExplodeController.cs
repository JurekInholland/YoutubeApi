using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Models;
using Models.DomainModels;
using Services.YoutubeExplodeService;
using YoutubeExplode.Channels;
using YoutubeExplode.Search;

namespace App.Controllers;

/// <summary>
/// Get information about various youtube resources via the YoutubeExplode library
/// </summary>
public class YoutubeExplodeController : BaseController
{
    private readonly ILogger<YoutubeExplodeController> _logger;
    private readonly IYoutubeExplodeService _youtubeExplodeService;

    /// <summary>
    /// YoutubeExplodeController Constructor
    /// </summary>
    public YoutubeExplodeController(ILogger<YoutubeExplodeController> logger, IYoutubeExplodeService youtubeExplodeService)
    {
        _logger = logger;
        _youtubeExplodeService = youtubeExplodeService;
    }

    /// <summary>
    /// Get YoutubeExplode info of a youtube video
    /// </summary>
    [HttpGet("video", Name = nameof(GetVideoInfo))]
    public async Task<IActionResult> GetVideoInfo(string videoId)
    {
        _logger.LogInformation("Getting video info for {VideoId}", videoId);
        try
        {
            YoutubeVideo video = await _youtubeExplodeService.GetVideo(videoId);
            _logger.LogInformation("Returning video info for {Video}", video.Title);
            return Ok(video);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Exception getting video info for {VideoId}: {Exception}", videoId, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Get YoutubeExplode info of a youtube channel
    /// </summary>
    [HttpGet("channel", Name = nameof(GetExplodeChannelInfo))]
    public async Task<IActionResult> GetExplodeChannelInfo(string channelId)
    {
        _logger.LogInformation("Getting channel info for {ChannelId}", channelId);
        try
        {
            IChannel channel = await _youtubeExplodeService.GetChannel(channelId);
            _logger.LogInformation("Returning channel info for {Channel}", channel.Title);
            return Ok(channel);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Exception getting channel info for {ChannelId}: {Exception}", channelId, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Search Youtube via YoutubeExplode
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("search", Name = nameof(GetSearchResults))]
    [ProducesResponseType(typeof(List<YoutubeSearchResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSearchResults(string query)
    {
        _logger.LogInformation("Getting search results for {Query}", query);
        try
        {
            var results = await _youtubeExplodeService.GetSearchResults(query);
            Response.Headers.Add("response-count", results.ToList().Count.ToString());
            _logger.LogInformation("Returning search results for {Query}", query);
            return Ok(results);
        }
        catch (Exception e)
        {
            _logger.LogInformation("Exception getting search results for {Query}: {Exception}", query, e.Message);
            return BadRequest(e.Message);
        }
    }
}
