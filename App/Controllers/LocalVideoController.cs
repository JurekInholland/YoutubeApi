using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DomainModels;
using Services.YoutubeService;

namespace App.Controllers;

/// <summary>
/// Controller to access local videos
/// </summary>
public class LocalVideoController : BaseController
{
    private readonly IYoutubeService _youtubeService;
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// LocalVideoController Constructor
    /// </summary>
    public LocalVideoController(IYoutubeService youtubeService, IUnitOfWork unitOfWork)
    {
        _youtubeService = youtubeService;
        _unitOfWork = unitOfWork;
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

    /// <summary>
    /// Get video stream
    /// </summary>
    [HttpGet(nameof(GetVideoStream))]
    public async Task<IActionResult> GetVideoStream(string videoId)
    {
        YoutubeVideo? video = await _unitOfWork.YoutubeVideos.Where(v => v.Id == videoId).Include(v => v.LocalVideo).FirstOrDefaultAsync();

        if (video?.LocalVideo is null || !System.IO.File.Exists(video.LocalVideo.Path))
        {
            return NotFound("Video not found");
        }

        return PhysicalFile(video.LocalVideo.Path, "video/webm", Path.GetFileName(video.LocalVideo.Path), true);
    }
}
