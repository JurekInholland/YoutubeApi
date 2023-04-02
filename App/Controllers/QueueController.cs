using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Models;
using Models.DomainModels;
using Models.Requests;
using Services.QueueService;

namespace App.Controllers;

/// <summary>
/// Handle queueing of downloads
/// </summary>
public class QueueController : BaseController
{
    private readonly ILogger<QueueController> _logger;
    private readonly IQueueService _queueService;

    /// <summary>
    /// QueueController constructor
    /// </summary>
    public QueueController(ILogger<QueueController> logger, IQueueService queueService)
    {
        _logger = logger;
        _queueService = queueService;
    }


    /// <summary>
    /// Get next enqueued video
    /// </summary>
    /// <returns></returns>
    [HttpGet("next", Name = nameof(GetNext))]
    public async Task<IActionResult> GetNext()
    {
        QueuedDownload? queuedDownload = await _queueService.DequeueDownload();
        return Ok(queuedDownload);
    }

    [HttpDelete("clear", Name = nameof(ClearQueue))]
    public async Task<IActionResult> ClearQueue()
    {
        await _queueService.ClearQueue();
        return Ok();
    }

    /// <summary>
    /// Get all enqueued videos
    /// </summary>
    [HttpGet("all", Name = nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var queuedDownloads = await _queueService.GetQueuedDownloads();
        return Ok(queuedDownloads);
    }

    /// <summary>
    /// Enqueue a video for download
    /// </summary>
    [OpenApiRequestBody("application/json", typeof(string), Description = "Youtube video id")]
    [HttpPost("add", Name = nameof(AddToQueue))]
    public async Task<IActionResult> AddToQueue([FromBody] EnqueueDownloadRequest request)
    {
        try
        {
            QueuedDownload queuedDownload = await _queueService.EnqueueDownload(request.VideoId);
            return Ok(queuedDownload);
        }
        catch (Exception e)
        {
            // _logger.LogError(e, "Error while adding video to queue");
            if (e is InvalidOperationException)
                return Conflict(e.Message);
            return BadRequest(e.Message);
        }
    }
}
