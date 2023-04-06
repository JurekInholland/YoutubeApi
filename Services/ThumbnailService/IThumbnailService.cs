namespace Services.ThumbnailService;

public interface IThumbnailService
{
    public Task<byte[]?> GetChannelThumbnail(string channelId);
    public Task<byte[]?> GetVideoThumbnail(string videoId);
}
