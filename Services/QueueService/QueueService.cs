using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using Models.DomainModels;
using Rnd;
using Services.ScrapeService;
using Services.YoutubeExplodeService;

namespace Services.QueueService;

public class QueueService : BackgroundService, IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYoutubeExplodeService _youtubeExplodeService;
    private readonly YoutubeHub _hub;
    private readonly IScrapeService _scrapeService;

    private CancellationTokenSource? _cancellationTokenSource;


    private static readonly Regex DownloadProgressRegex =
        new(
            @"\[(?<kind>\w+)\]\s+(?<progress>\d+\.\d+)%\s+of\s+~\s+(?<size>\d+\.\d+)\s*(?<unit>[a-zA-Z]+)\s+at\s+(?<speed>\d+\.\d+)\s*(?<speedUnit>[a-zA-Z]+\/s)\s+ETA\s+(?<eta>\d{2}:\d{2})\s+\(frag\s+(?<frag>\d+\/\d+)\)");


    private DateTime _lastExecution = DateTime.MinValue;

    public QueueService(ILogger<QueueService> logger, IServiceScopeFactory serviceScopeFactory,
        YoutubeHub hub, IScrapeService scrapeService)
    {
        IServiceScope scope = serviceScopeFactory.CreateScope();

        _logger = logger;
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _youtubeExplodeService = scope.ServiceProvider.GetRequiredService<IYoutubeExplodeService>();
        _hub = hub;
        _scrapeService = scrapeService;
    }

    public Task StopAsync()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        return Task.CompletedTask;
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

    public async Task ClearCompleted()
    {
        await _unitOfWork.QueuedDownloads.DeleteByStatus(Enums.DownloadStatus.Finished);
        await _unitOfWork.Save();
    }

    public async Task DeleteFromQueue(string videoId)
    {
        _unitOfWork.QueuedDownloads.DeleteById(videoId);
        await _unitOfWork.Save();
    }


    public async Task ProcessQueue(CancellationToken cancellationToken)
    {
        try
        {
            await _hub.SendTaskUpdate(Enums.ApplicationTask.ProcessDownloadQueue, Enums.TaskStatus.Started);

            await DownloadQueueVideos(cancellationToken);
        }
        catch (Exception e)
        {
            await _hub.SendTaskUpdate(Enums.ApplicationTask.ProcessDownloadQueue, Enums.TaskStatus.Error);

            _logger.LogError(e, "Error processing queue");
        }
        finally
        {
            await _hub.SendTaskUpdate(Enums.ApplicationTask.ProcessDownloadQueue, Enums.TaskStatus.Finished);

            Console.WriteLine("QUEUE PROCESSED");
        }
    }

    public async Task ResetQueue()
    {
        var done = await _unitOfWork.QueuedDownloads.GetByStatus(Enums.DownloadStatus.Finished);
        foreach (var queuedDownload in done)
        {
            queuedDownload.Status = Enums.DownloadStatus.Queued;
        }

        await _unitOfWork.Save();
    }

    private static async Task<LocalVideo?> ReadDownloadedVideoJson(QueuedDownload queuedDownload)
    {
        // var settings = await _unitOfWork.ApplicationSettings.GetSettings();

        var jsonPath =
            $"data/videos/{queuedDownload.Video.YoutubeChannel.Title}/{queuedDownload.Id} - {queuedDownload.Video.Title}.info.json"
                .Replace(":", "_");

        var jsonFile = await File.ReadAllTextAsync(jsonPath);
        var doc = JsonDocument.Parse(jsonFile);

        int width = doc.RootElement.GetProperty("width").GetInt32();
        int height = doc.RootElement.GetProperty("height").GetInt32();
        int fps = doc.RootElement.GetProperty("fps").GetInt32();
        float vbr = doc.RootElement.GetProperty("vbr").GetSingle();
        float abr = doc.RootElement.GetProperty("abr").GetSingle();
        string ext = doc.RootElement.GetProperty("ext").GetString()!;
        int size = doc.RootElement.GetProperty("filesize_approx").GetInt32();

        var videoPath =
            $"data/videos/{queuedDownload.Video.YoutubeChannel.Title}/{queuedDownload.Video.Id} - {queuedDownload.Video.Title}.{ext}";

        LocalVideo localVideo = new()
        {
            Id = queuedDownload.Video.Id,
            Path = videoPath,
            Width = width,
            Height = height,
            Fps = fps,
            Vbr = vbr,
            Abr = abr,
            Extension = ext,
            Size = size
        };
        return localVideo;
    }

    private async Task DownloadQueueVideos(CancellationToken cancellationToken)
    {
        Console.WriteLine("DownloadQueueVideos");
        QueuedDownload? queuedDownload = await DequeueDownload();


        Console.WriteLine("DL " + queuedDownload?.Video.Title);

        string format = "data/videos/%(uploader)s/%(id)s - %(title)s.%(ext)s";

        while (queuedDownload is not null && !cancellationToken.IsCancellationRequested)
        {
            await YoutubeDownloader.DownloadVideo(queuedDownload.Video.Id, format, DownloadCallback);
            LocalVideo? local = await ReadDownloadedVideoJson(queuedDownload);

            if (queuedDownload.Video.LocalVideo is not null)
            {
                Console.WriteLine("Video already downloaded");

                _unitOfWork.LocalVideos.Delete(queuedDownload.Video.LocalVideo);
                // queuedDownload.Video.LocalVideo = null;
                await _unitOfWork.Save();
            }


            queuedDownload.Video.LocalVideo = local;
            await _unitOfWork.LocalVideos.Create(local!);
            _unitOfWork.YoutubeVideos.Update(queuedDownload.Video);
            Console.WriteLine("dl done 1");
            queuedDownload.Status = Enums.DownloadStatus.Finished;
            _unitOfWork.QueuedDownloads.Update(queuedDownload);
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
            // await _hub.SendObject("user", "downloadProgress", finalProgress);
            await _hub.SendDownloadProgress(finalProgress);
            return;
        }

        if (content is null || delta < TimeSpan.FromSeconds(.2)) return;

        DownloadProgress? progress = ParseProgressLine(content, videoId);
        if (progress is null) return;

        // await _hub.SendObject("user", "downloadProgress", progress);
        await _hub.SendDownloadProgress(progress);

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
        // float size = float.Parse(match.Groups["size"].Value);
        // string unit = match.Groups["unit"].Value;
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
            _unitOfWork.YoutubeChannels.Delete(existing.YoutubeChannel);
            _unitOfWork.YoutubeVideos.Delete(existing);
            await _unitOfWork.Save();
        }

        // YoutubeVideo video = await _youtubeExplodeService.GetVideo(videoId);
        YoutubeVideo video = await _scrapeService.ScrapeYoutubeVideo(videoId);

        if (video.Duration > TimeSpan.FromMinutes(60))
        {
            throw new InvalidOperationException("Video is too long");
        }

        QueuedDownload queuedDownload = new()
        {
            Id = videoId,
            Status = Enums.DownloadStatus.Queued,
            QueuedAt = DateTime.Now,
            // YoutubeVideoId = video.Id,
            Video = video
        };


        await _unitOfWork.QueuedDownloads.Create(queuedDownload);
        try
        {
            await _unitOfWork.Save();
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR");
            Console.WriteLine(e);
        }

        // queuedDownload.Video = video;
        return queuedDownload;
    }

    public async Task<QueuedDownload?> DequeueDownload()
    {
        var queuedDownloads = _unitOfWork.QueuedDownloads.All().Include(q => q.Video).ThenInclude(v => v.YoutubeChannel);
        if (!queuedDownloads.Any())
        {
            return null;
        }

        QueuedDownload? queuedDownload = await queuedDownloads.Include(q => q.Video).ThenInclude(v => v.LocalVideo)
            .OrderBy(x => x.QueuedAt)
            .FirstOrDefaultAsync(x => x.Status == Enums.DownloadStatus.Queued);

        return queuedDownload;
    }

    public async Task CallExecuteAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("QUEUESEVICE EXECUTE ASYNC");
        await DownloadQueueVideos(stoppingToken);
    }
}
