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

namespace Services.QueueService;

public class QueueService : BackgroundService, IQueueService
{
    private readonly ILogger<QueueService> _logger;
    private readonly YoutubeHub _hub;
    private readonly IScrapeService _scrapeService;
    private CancellationTokenSource? _cancellationTokenSource;


    private IUnitOfWork UnitOfWork => _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

    private static readonly Regex DownloadProgressRegex =
        new(
            @"\[(?<kind>\w+)\]\s+(?<progress>\d+\.\d+)%\s+of\s+~\s+(?<size>\d+\.\d+)\s*(?<unit>[a-zA-Z]+)\s+at\s+(?<speed>\d+\.\d+)\s*(?<speedUnit>[a-zA-Z]+\/s)\s+ETA\s+(?<eta>\d{2}:\d{2})\s+\(frag\s+(?<frag>\d+\/\d+)\)");


    private DateTime _lastExecution = DateTime.MinValue;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public QueueService(ILogger<QueueService> logger, IServiceScopeFactory serviceScopeFactory,
        YoutubeHub hub, IScrapeService scrapeService)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
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
        return await UnitOfWork.QueuedDownloads.All().Include(q => q.Video).ThenInclude(v => v.YoutubeChannel).ToListAsync();
    }

    public async Task ClearQueue()
    {
        IUnitOfWork unitOfWork = UnitOfWork;

        unitOfWork.QueuedDownloads.DeleteAll();
        await unitOfWork.Save();
    }

    public async Task ClearCompleted()
    {
        IUnitOfWork unitOfWork = UnitOfWork;

        await unitOfWork.QueuedDownloads.DeleteByStatus(Enums.DownloadStatus.Finished);
        await unitOfWork.Save();
    }

    public async Task DeleteFromQueue(string videoId)
    {
        IUnitOfWork unitOfWork = UnitOfWork;
        unitOfWork.QueuedDownloads.DeleteById(videoId);
        await unitOfWork.Save();
    }


    public async Task ProcessQueue(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing queue");
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
        IUnitOfWork unitOfWork = UnitOfWork;
        var done = await unitOfWork.QueuedDownloads.GetByStatus(Enums.DownloadStatus.Finished);
        foreach (var queuedDownload in done)
        {
            queuedDownload.Status = Enums.DownloadStatus.Queued;
        }

        await unitOfWork.Save();
    }

    private static async Task<LocalVideo?> ReadDownloadedVideoJson(QueuedDownload queuedDownload)
    {
        Console.WriteLine("Reading json file");

        var targetDir = Path.GetFullPath($"data/videos/{queuedDownload.Video.YoutubeChannel.Title}");
        var files = Directory.GetFiles(targetDir, $"{queuedDownload.Id}*.info.json");

        string? file = files.FirstOrDefault();

        if (file is null)
        {
            throw new($"Could not find json file for video with id {queuedDownload.Id}");
        }

        string jsonFile = await File.ReadAllTextAsync(file);
        JsonDocument doc = JsonDocument.Parse(jsonFile);

        int width = doc.RootElement.GetProperty("width").GetInt32();
        int height = doc.RootElement.GetProperty("height").GetInt32();
        int fps = doc.RootElement.GetProperty("fps").GetInt32();
        float vbr = doc.RootElement.GetProperty("vbr").GetSingle();
        float abr = doc.RootElement.GetProperty("abr").GetSingle();
        string ext = doc.RootElement.GetProperty("ext").GetString()!;
        long size = 0;

        if (doc.RootElement.TryGetProperty("filesize_approx", out var fileSizeApprox))
        {
            fileSizeApprox.TryGetInt64(out size);
        }

        if (doc.RootElement.TryGetProperty("filesize", out var fileSize))
        {
            fileSize.TryGetInt64(out size);
        }

        var videoPath = file.Replace(".info.json", $".{ext}");
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
        IUnitOfWork unitOfWork = UnitOfWork;

        Console.WriteLine("DownloadQueueVideos");
        QueuedDownload? queuedDownload = await DequeueDownload();


        Console.WriteLine("DL " + queuedDownload?.Video.Title);

        const string format = "data/videos/%(uploader)s/%(id)s - %(title)s.%(ext)s";

        while (queuedDownload is not null && !cancellationToken.IsCancellationRequested)
        {
            await YoutubeDownloader.DownloadVideo(queuedDownload.Video.Id, format, DownloadCallback);
            LocalVideo? local = await ReadDownloadedVideoJson(queuedDownload);

            if (queuedDownload.Video.LocalVideo is not null)
            {
                Console.WriteLine("Video already downloaded");

                unitOfWork.LocalVideos.Delete(queuedDownload.Video.LocalVideo);
                await unitOfWork.Save();
            }


            queuedDownload.Video.LocalVideo = local;
            Console.WriteLine("dl done 1");
            queuedDownload.Status = Enums.DownloadStatus.Finished;
            await unitOfWork.QueuedDownloads.UpdateQueueVideo(queuedDownload);

            await _hub.SendLocalVideo(local!);

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

            await _hub.SendDownloadProgress(finalProgress);
            return;
        }

        if (content is null || delta < TimeSpan.FromSeconds(.2)) return;

        DownloadProgress? progress = ParseProgressLine(content, videoId);
        if (progress is null) return;

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
        var unitOfWork = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (videoId is null or "")
        {
            throw new ArgumentNullException(nameof(videoId));
        }

        var queued = unitOfWork.QueuedDownloads.Where(x => x.Id == videoId);
        if (queued.Any())
        {
            throw new InvalidOperationException("Video already in queue");
        }

        YoutubeVideo? video = await _scrapeService.ScrapeYoutubeVideo(videoId);

        if (video?.Duration > TimeSpan.FromMinutes(60))
        {
            throw new InvalidOperationException("Video is too long");
        }

        QueuedDownload queuedDownload = new()
        {
            Id = videoId,
            Status = Enums.DownloadStatus.Queued,
            QueuedAt = DateTime.Now,
            Video = video!
        };


        try
        {
            await unitOfWork.QueuedDownloads.EnqueueDownload(queuedDownload);
        }
        catch (Exception e)
        {
            Console.WriteLine("ERROR");
            Console.WriteLine(e);
        }

        return queuedDownload;
    }

    public async Task<QueuedDownload?> DequeueDownload()
    {
        var queuedDownloads = UnitOfWork.QueuedDownloads.All();
        Console.WriteLine("brk");
        if (!queuedDownloads.Any())
        {
            return null;
        }

        QueuedDownload? queuedDownload = await queuedDownloads
            .Include(q => q.Video)
            .ThenInclude(v => v.LocalVideo)
            .Include(q => q.Video)
            .ThenInclude(v => v.YoutubeChannel)
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
