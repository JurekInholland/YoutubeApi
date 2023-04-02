using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Services.YoutubeExploseService;

namespace Services.QueueService;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYoutubeExplodeService _youtubeExplodeService;

    public QueueService(ILogger<QueueService> logger, IUnitOfWork unitOfWork, IYoutubeExplodeService youtubeExplodeService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _youtubeExplodeService = youtubeExplodeService;
    }

    public async Task<IEnumerable<QueuedDownload>> GetQueuedDownloads()
    {
        return await _unitOfWork.QueuedDownloads.All().ToListAsync();
    }

    public async Task<QueuedDownload> EnqueueDownload(string videoId)
    {

        var queued = _unitOfWork.QueuedDownloads.Where(x => x.Id == videoId);
        if (queued.Any())
        {
            throw new InvalidOperationException("Video already in queue");
        }

        YoutubeVideo video = await _youtubeExplodeService.GetVideo(videoId);

        _logger.LogInformation("Enqueuing video {VideoId} - {VideoTitle}", video.Id, video.Title);

        QueuedDownload queuedDownload = new()
        {
            Id = videoId,
            Status = Enums.DownloadStatus.Queued,
            QueuedAt = DateTime.Now,
            Video = video
        };

        _unitOfWork.QueuedDownloads.Create(queuedDownload);
        await _unitOfWork.Save();

        return queuedDownload;
    }

    public async Task<QueuedDownload?> DequeueDownload()
    {
        var queuedDownloads = _unitOfWork.QueuedDownloads.All();
        if (!queuedDownloads.Any())
        {
            return null;
        }

        QueuedDownload? queuedDownload = await queuedDownloads
            .OrderBy(x => x.QueuedAt)
            .FirstOrDefaultAsync(x => x.Status == Enums.DownloadStatus.Queued);

        return queuedDownload;
    }
}
