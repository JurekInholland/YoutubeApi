using Microsoft.AspNetCore.Mvc;
using Services.YoutubeService;

namespace App.Controllers;

public class LocalVideoController : BaseController
{
    private readonly IYoutubeService _youtubeService;

    public LocalVideoController(IYoutubeService youtubeService)
    {
        _youtubeService = youtubeService;
    }

    /// <summary>
    /// Get all locally available videos
    /// </summary>
    [HttpGet(Name = "GetLocalVideos")]
    public async Task<IActionResult> GetLocalVideos()
    {
        var videos = await _youtubeService.GetLocalVideos();
        return Ok(videos);
    }
}
