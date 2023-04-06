using Microsoft.AspNetCore.Mvc;
using Services.ThumbnailService;

namespace App.Controllers;

public class ThumbnailController : BaseController
{
    private readonly ILogger<ThumbnailController> _logger;
    private readonly IThumbnailService _thumbnailService;

    public ThumbnailController(ILogger<ThumbnailController> logger, IThumbnailService thumbnailService)
    {
        _logger = logger;
        _thumbnailService = thumbnailService;
    }

    /// <summary>
    /// Get the thumbnail for a youtube channel
    /// </summary>
    [HttpGet("channel", Name = nameof(GetChannelThumbnail))]
    public async Task<IActionResult> GetChannelThumbnail(string channelId)
    {
        try
        {
            var file = await _thumbnailService.GetChannelThumbnail(channelId);
            return File(file!, "image/jpeg");
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Error getting channel thumbnail");
            return BadRequest(e.Message);
        }
    }
}
