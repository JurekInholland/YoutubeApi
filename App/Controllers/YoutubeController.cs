using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Rnd;
using Services.YoutubeExploseService;
using Services.YoutubeService;

namespace App.Controllers;

/// <summary>
/// YoutubeController
/// </summary>
public class YoutubeController : BaseController
{
    private readonly ILogger<YoutubeController> _logger;
    private readonly IYoutubeService _youtubeService;
    private readonly IYoutubeExplodeService _youtubeExplodeService;


    /// <summary>
    /// YoutubeController constructor
    /// </summary>
    public YoutubeController(ILogger<YoutubeController> logger, IYoutubeService youtubeService,
        IYoutubeExplodeService youtubeExplodeService)
    {
        _logger = logger;
        _youtubeService = youtubeService;
        _youtubeExplodeService = youtubeExplodeService;
    }

    /// <summary>
    /// Get YoutubeExplode info of a youtube video
    /// </summary>
    [HttpGet("explodeChannel", Name = nameof(GetExplodeChannel))]
    public async Task<IActionResult> GetExplodeChannel(string channelId)
    {
        var chan = await _youtubeExplodeService.GetChannel(channelId);
        return Ok(chan);
    }


    /// <summary>
    /// Get YoutubeExplode info of a youtube video
    /// </summary>
    [HttpGet("explodeRelated", Name = nameof(GetExplodeRelated))]
    public async Task<IActionResult> GetExplodeRelated(string videoId)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var related = await _youtubeExplodeService.GetRelatedVideos(videoId);
        stopwatch.Stop();
        Response.Headers.Add("X-Elapsed-Milliseconds", stopwatch.ElapsedMilliseconds.ToString());
        return Ok(related);
    }

    /// <summary>
    /// Download video via YoutubeExplode
    /// </summary>
    [HttpGet("explodeDl", Name = nameof(GetExplodeDownload))]
    public async Task<IActionResult> GetExplodeDownload(string videoId)
    {
        await _youtubeExplodeService.DownloadVideo(videoId);
        return Ok("download");
    }

    /// <summary>
    /// Get video info via YoutubeExplode
    /// </summary>
    [HttpGet("explodeInfo", Name = nameof(GetExplodeInfo))]
    public async Task<IActionResult> GetExplodeInfo(string videoId)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var video = await _youtubeExplodeService.GetVideo(videoId);
        stopwatch.Stop();
        Response.Headers.Add("X-Elapsed-Milliseconds", stopwatch.ElapsedMilliseconds.ToString());
        return Ok(video);
    }

    /// <summary>
    /// Get video info via YoutubeDownloader
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    [HttpGet("video", Name = nameof(GetVideoInfo))]
    public async Task<IActionResult> GetVideoInfo(string videoId)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var info = await YoutubeDownloader.GetVideoInfo(videoId);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Deserialize and pretty-print JSON string
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(info);
        string prettyJson = JsonSerializer.Serialize(jsonElement, options);
        stopwatch.Stop();
        Response.Headers.Add("X-Elapsed-Milliseconds", stopwatch.ElapsedMilliseconds.ToString());
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
