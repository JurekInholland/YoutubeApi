using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.YoutubeService;

namespace App.Controllers;

public class LocalVideoController : BaseController
{
    private readonly IYoutubeService _youtubeService;
    private readonly IUnitOfWork _unitOfWork;

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
        var local = await _unitOfWork.LocalVideos.Where(l => l.Id == videoId).FirstOrDefaultAsync();

        if (local is null)
        {
            return NotFound("Video not found");
        }

        var filestream = System.IO.File.OpenRead(local.Path);
        return File(filestream, "video/mkv", fileDownloadName: "videotest.mkv", enableRangeProcessing: true);
    }
}
