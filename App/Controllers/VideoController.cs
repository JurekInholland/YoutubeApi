using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class VideoController : BaseController
{
    private readonly ILogger<VideoController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public VideoController(ILogger<VideoController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    /// <summary>
    /// Get a backed up video file
    /// </summary>
    [HttpGet("", Name = nameof(GetVideo))]
    public async Task<IActionResult> GetVideo(string videoId)
    {
        var video = await _unitOfWork.YoutubeVideos.Where(x => x.Id == videoId).FirstOrDefaultAsync();
        if (video?.LocalVideo is null) return NotFound();

        var videoFile = new FileInfo(video.LocalVideo.Path);
        if (!videoFile.Exists) return NotFound();

        return PhysicalFile(videoFile.FullName, "video/mp4");
    }
}
