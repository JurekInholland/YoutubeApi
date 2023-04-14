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
        // var local = await _unitOfWork.LocalVideos.Where(l => l.Id == videoId).FirstOrDefaultAsync();

        var video = await _unitOfWork.YoutubeVideos.Where(v => v.Id == videoId).Include(v => v.LocalVideo).FirstOrDefaultAsync();

        if (video?.LocalVideo is null)
        {
            return NotFound("Video not found");
        }

        var filestream = System.IO.File.OpenRead(video.LocalVideo.Path);
        return File(filestream, "video/mkv", fileDownloadName: video.Id + "." + video.LocalVideo.Extension, enableRangeProcessing: true);
    }
}
