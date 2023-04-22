using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Services.ThumbnailService;

namespace App.Controllers;

/// <summary>
/// ThumbnailController
/// </summary>
public class ThumbnailController : BaseController
{
    private readonly ILogger<ThumbnailController> _logger;
    private readonly IThumbnailService _thumbnailService;

    /// <summary>
    /// ThumbnailController constructor
    /// </summary>
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
        Console.WriteLine("GetChannelThumbnail");
        try
        {
            byte[]? file = await _thumbnailService.GetChannelThumbnail(channelId);
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

    /// <summary>
    /// Get the banner image of a youtube channel
    /// </summary>
    /// <param name="channelId">Id of the youtube channel</param>
    /// <returns>Banner image</returns>
    [HttpGet(nameof(ChannelBanner))]
    [ProducesResponseType(typeof(FileStreamResult), 200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChannelBanner(string channelId)
    {
        try
        {
            byte[]? file = await _thumbnailService.GetChannelBanner(channelId);
            return File(file!, "image/jpeg");
        }
        catch (FileNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}
