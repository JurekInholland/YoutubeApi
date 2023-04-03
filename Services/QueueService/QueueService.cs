using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.DomainModels;
using Services.YoutubeExplodeService;

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
        return await _unitOfWork.QueuedDownloads.All().Include(q => q.Video).ToListAsync();
    }

    public async Task ClearQueue()
    {
        _unitOfWork.QueuedDownloads.DeleteAll();
        await _unitOfWork.Save();
    }

    public async Task DeleteFromQueue(string videoId)
    {
        _unitOfWork.QueuedDownloads.DeleteById(videoId);
        await _unitOfWork.Save();
    }

    public async Task<QueuedDownload> EnqueueDownload(string videoId)
    {
        var queued = _unitOfWork.QueuedDownloads.Where(x => x.Id == videoId);
        if (queued.Any())
        {
            throw new InvalidOperationException("Video already in queue");
        }

        YoutubeVideo? existing = await _unitOfWork.YoutubeVideos.Where(x => x.Id == videoId).FirstOrDefaultAsync();

        if (existing is not null)
        {
            _unitOfWork.YoutubeVideos.Delete(existing);
            await _unitOfWork.Save();
        }

        YoutubeVideo video = await _youtubeExplodeService.GetVideo(videoId);

        QueuedDownload queuedDownload = new()
        {
            Id = videoId,
            Status = Enums.DownloadStatus.Queued,
            QueuedAt = DateTime.Now,
            // YoutubeVideoId = video.Id,
            Video = video
        };


        await _unitOfWork.QueuedDownloads.Create(queuedDownload);
        await _unitOfWork.Save();
        // queuedDownload.Video = video;
        return queuedDownload;
    }

    public async Task<QueuedDownload?> DequeueDownload()
    {
        var queuedDownloads = _unitOfWork.QueuedDownloads.All();
        if (!queuedDownloads.Any())
        {
            return null;
        }

        QueuedDownload? queuedDownload = await queuedDownloads.Include(q => q.Video)
            .OrderBy(x => x.QueuedAt)
            .FirstOrDefaultAsync(x => x.Status == Enums.DownloadStatus.Queued);

        return queuedDownload;
    }
}
