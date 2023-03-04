using Microsoft.AspNetCore.Mvc;
using Services.YoutubeService;

namespace App.Controllers;

/// <summary>
/// Get json info of youtube objects such as videos, playlists and channels
/// </summary>
public class InfoController : BaseController
{
    private readonly ILogger<InfoController> _logger;
    private readonly IYoutubeService _youtubeService;

    public InfoController(ILogger<InfoController> logger, IYoutubeService youtubeService)
    {
        _logger = logger;
        _youtubeService = youtubeService;
    }


    /// <summary>
    /// Check if a youtube video id is valid
    /// </summary>
    [HttpGet(nameof(CheckId))]
    public async Task<IActionResult> CheckId(string id)
    {
        var valid = await _youtubeService.IsValidId(id);
        return Ok(valid ? "Valid" : "Invalid");
    }

    /// <summary>
    /// Get json info of a youtube video
    /// </summary>
    /// <param name="videoId"></param>
    /// <returns></returns>
    [HttpGet(nameof(GetVideoInfo))]
    public async Task<IActionResult> GetVideoInfo(string videoId)
    {
        var valid = await _youtubeService.IsValidId(videoId);
        if (!valid) return BadRequest("Invalid videoId");

        var info = await _youtubeService.GetVideoInfo(videoId);

        // if (!videoId.IsValidYoutubeId()) return BadRequest("Invalid videoId");
        // var videoInfo = await YoutubeDownloader.GetVideoInfo(videoId);
        return Ok(info);
    }


    /// <summary>
    /// Get full yt-dlp json info of a youtube video
    /// </summary>
    [HttpGet(nameof(GetFullInfo))]
    public async Task<IActionResult> GetFullInfo(string videoId)
    {
        var info = await _youtubeService.GetFullInfo(videoId);
        return Ok(info);
    }


    /// <summary>
    /// Ping pong
    /// </summary>
    [HttpGet(nameof(GetPing))]
    public IActionResult GetPing()
    {
        return Ok("pong");
    }
}
