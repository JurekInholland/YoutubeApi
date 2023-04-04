using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Domain;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using Models.DomainModels;
using Rnd;
using Services.YoutubeExplodeService;

namespace Services.QueueService;

public class QueueService : IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYoutubeExplodeService _youtubeExplodeService;
    private readonly YoutubeHub _hub;

    private static readonly Regex DownloadProgressRegex =
        new(
            @"\[(?<kind>\w+)\]\s+(?<progress>\d+\.\d+)%\s+of\s+~\s+(?<size>\d+\.\d+)\s*(?<unit>[a-zA-Z]+)\s+at\s+(?<speed>\d+\.\d+)\s*(?<speedUnit>[a-zA-Z]+\/s)\s+ETA\s+(?<eta>\d{2}:\d{2})\s+\(frag\s+(?<frag>\d+\/\d+)\)");


    private DateTime _lastExecution = DateTime.MinValue;

    public QueueService(ILogger<QueueService> logger, IUnitOfWork unitOfWork, IYoutubeExplodeService youtubeExplodeService, YoutubeHub hub)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _youtubeExplodeService = youtubeExplodeService;
        _hub = hub;
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

    public async Task ProcessQueue()
    {
        await DownloadQueueVideos();
    }

    private async ValueTask DownloadQueueVideos()
    {
        Console.WriteLine("DownloadQueueVideos");
        QueuedDownload? queuedDownload = await DequeueDownload();

        Console.WriteLine("DL " + queuedDownload?.Video.Title);
        while (queuedDownload is not null)
        {
            await YoutubeDownloader.DownloadVideo(queuedDownload.Video.Id, DownloadCallback);
            Console.WriteLine("dl done 1");
            queuedDownload.Status = Enums.DownloadStatus.Finished;
            // _unitOfWork.QueuedDownloads.Update(queuedDownload);
            await _unitOfWork.Save();
            queuedDownload = await DequeueDownload();
            Console.WriteLine("qd");
        }

        Console.WriteLine("dl done 2");
    }

    private async Task DownloadCallback(string? content, string videoId)
    {
        Console.WriteLine("content: " + content);
        TimeSpan delta = DateTime.Now - _lastExecution;

        if (content is not null && content.Contains("[Metadata] Adding metadata to"))
        {
            DownloadProgress finalProgress = new()
            {
                Id = videoId,
                Progress = 100,
                Status = "finished"
            };
            await _hub.SendObjects("user", "downloadProgress", finalProgress);
            return;
        }

        if (content is null || delta < TimeSpan.FromSeconds(.2)) return;

        DownloadProgress? progress = ParseProgressLine(content, videoId);
        if (progress is null) return;

        await _hub.SendObjects("user", "downloadProgress", progress);

        _lastExecution = DateTime.Now;
    }

    private static DownloadProgress? ParseProgressLine(string line, string videoId)
    {
        if (line is null or "")
        {
            return null;
        }

        Match match = DownloadProgressRegex.Match(line);
        if (!match.Success) return null;

        string kind = match.Groups["kind"].Value;
        float progress = float.Parse(match.Groups["progress"].Value, CultureInfo.InvariantCulture);
        float size = float.Parse(match.Groups["size"].Value);
        string unit = match.Groups["unit"].Value;
        float speed = float.Parse(match.Groups["speed"].Value.Replace('.', ','));
        string speedUnit = match.Groups["speedUnit"].Value;
        TimeSpan eta = TimeSpan.ParseExact(match.Groups["eta"].Value, "mm':'ss", CultureInfo.InvariantCulture);
        string frag = match.Groups["frag"].Value;

        return new DownloadProgress
        {
            Id = videoId,
            Status = kind,
            Progress = progress,
            Speed = speed + speedUnit,
            Eta = eta,
            Fragment = frag
        };
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
