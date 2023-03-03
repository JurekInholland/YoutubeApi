using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Rnd;
using Services.YoutubeService;

namespace App.Controllers;

public class YoutubeController : BaseController
{
    private readonly ILogger<YoutubeController> _logger;
    private readonly IYoutubeService _youtubeService;

    /// <summary>
    /// YoutubeController
    /// </summary>
    public YoutubeController(ILogger<YoutubeController> logger, IYoutubeService youtubeService)
    {
        _logger = logger;
        _youtubeService = youtubeService;
    }

    [HttpGet("video", Name = nameof(GetVideoInfo))]
    public async Task<IActionResult> GetVideoInfo(string videoId)
    {
        var info = await YoutubeDownloader.GetVideoInfo(videoId);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Deserialize and pretty-print JSON string
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(info);
        string prettyJson = JsonSerializer.Serialize(jsonElement, options);
        return Ok(prettyJson);
    }

    /// <summary>
    /// This is summary
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns>returns value</returns>
    [Description("Download video by videoId")]
    [HttpPut("video", Name = nameof(DownloadVideo))]
    public async Task<IActionResult> DownloadVideo(string videoId)
    {
        var info = await YoutubeDownloader.DownloadVideo(videoId);
        return Ok(info);
    }

    /// <summary>
    /// Returns a collection of URLs
    /// </summary>
    /// <param name="slugs">list of slugs to retrieve</param>
    /// <remarks>
    /// Sample request:
    ///
    ///     Get /api/slugscollection/(d25tRx, fN5jpz)
    ///
    /// </remarks>
    /// <returns>IEnumerable of slugs</returns>
    /// <response code="200">If all requested items are
    /// found</response>
    /// <response code="400">If slugs parameter is missing</response>
    /// <response code="404">If number of records found doesn't equal
    /// number of records requested</response>
    [Produces("application/json")]
    [HttpGet("channel", Name = nameof(GetChannelInfo))]
    public async Task<IActionResult> GetChannelInfo(string channelId)
    {
        var info = await YoutubeDownloader.GetChannelInfo(channelId);
        return Ok(info);

        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Deserialize and pretty-print JSON string
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(info);
        string prettyJson = JsonSerializer.Serialize(jsonElement, options);
        return Ok(prettyJson);
    }
}
