using Microsoft.Extensions.Logging;
using Services.ScrapeService;
using Services.YoutubeExplodeService;

namespace Services.ThumbnailService;

public class ThumbnailService : IThumbnailService
{
    private readonly ILogger<ThumbnailService> _logger;
    private readonly IYoutubeExplodeService _youtubeExplodeService;
    private readonly IScrapeService _scrapeService;

    public ThumbnailService(ILogger<ThumbnailService> logger, IYoutubeExplodeService youtubeExplodeService, IScrapeService scrapeService)
    {
        _logger = logger;
        _youtubeExplodeService = youtubeExplodeService;
        _scrapeService = scrapeService;
    }

    public async Task<byte[]?> GetChannelThumbnail(string channelId)
    {
        var path = $"data/thumbnails/{channelId}.jpg";

        if (!File.Exists(path))
        {
            await _scrapeService.DownloadChannelThumbnail(channelId);
            // await _youtubeExplodeService.GetChannel(channelId);
        }


        var file = await File.ReadAllBytesAsync(path);
        return file;
    }


    public async Task<byte[]?> GetVideoThumbnail(string videoId)
    {
        var path = $"data/thumbnails/videos/{videoId}.jpg";

        var file = await File.ReadAllBytesAsync(path);
        return file;
    }
}
