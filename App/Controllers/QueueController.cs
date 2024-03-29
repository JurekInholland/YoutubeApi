using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// QueueController constructor
    /// </summary>
    public QueueController(ILogger<QueueController> logger, IQueueService queueService, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _queueService = queueService;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Debug; Reset queue status of all items
    /// </summary>
    /// <returns></returns>
    [HttpGet(nameof(Reset))]
    public async Task<IActionResult> Reset()
    {
        await _queueService.ResetQueue();
        return Ok();
    }


    /// <summary>
    /// Process the queue
    /// </summary>
    [HttpGet(nameof(Process))]
    public async Task<IActionResult> Process()
    {
        var currentTask = (await _unitOfWork.ApplicationSettings.GetSettings()).CurrentTask;
        if (currentTask != null)
        {
            _logger.LogWarning("Task already running " + currentTask);
            return BadRequest("Task already running");
        }

        Task.Run(() => _queueService.ProcessQueue(CancellationToken.None));
        _logger.LogInformation("Started processing queue");
        return Ok();
    }

    /// <summary>
    /// Get next enqueued video
    /// </summary>
    /// <returns></returns>
    [HttpGet(nameof(Next))]
    public async Task<IActionResult> Next()
    {
        QueuedDownload? queuedDownload = await _queueService.DequeueDownload();
        return Ok(queuedDownload);
    }

    /// <summary>
    /// Clear the queue
    /// </summary>
    [HttpDelete(nameof(Clear))]
    public async Task<IActionResult> Clear()
    {
        await _queueService.ClearQueue();
        return Ok();
    }

    /// <summary>
    /// Get all enqueued videos
    /// </summary>
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll()
    {
        var queuedDownloads = await _queueService.GetQueuedDownloads();
        return Ok(queuedDownloads);
    }

    /// <summary>
    /// Enqueue a video for download
    /// </summary>
    [HttpPost(nameof(Add))]
    public async Task<IActionResult> Add([FromBody] EnqueueDownloadRequest request)
    {
        try
        {
            QueuedDownload queuedDownload = await _queueService.EnqueueDownload(request.VideoId);
            return Ok(queuedDownload);
        }
        catch (Exception e)
        {
            if (e is InvalidOperationException)
            {
                return Conflict(e.Message);
            }

            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Remove a queued video from the queue
    /// </summary>
    [HttpDelete("{videoId}", Name = nameof(DeleteFromQueue))]
    public async Task<IActionResult> DeleteFromQueue(string videoId)
    {
        _logger.LogInformation("Deleting video {VideoId} from queue", videoId);
        await _queueService.DeleteFromQueue(videoId);
        return Ok();
    }
}
