using Microsoft.Extensions.Logging;
using Services.YoutubeExplodeService;

namespace Services.ThumbnailService;

public class ThumbnailService : IThumbnailService
{
    private readonly ILogger<ThumbnailService> _logger;
    private readonly IYoutubeExplodeService _youtubeExplodeService;

    public ThumbnailService(ILogger<ThumbnailService> logger, IYoutubeExplodeService youtubeExplodeService)
    {
        _logger = logger;
        _youtubeExplodeService = youtubeExplodeService;
    }

    public async Task<byte[]?> GetChannelThumbnail(string channelId)
    {
        var path = $"data/thumbnails/{channelId}.jpg";

        if (!File.Exists(path))
        {
            await _youtubeExplodeService.GetChannel(channelId);
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
