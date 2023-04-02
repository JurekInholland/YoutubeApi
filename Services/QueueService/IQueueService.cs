using Models;

namespace Services.QueueService;

public interface IQueueService
{
    public Task<IEnumerable<QueuedDownload>> GetQueuedDownloads();

    public Task<QueuedDownload> EnqueueDownload(string videoId);
    // public Task EnqueueChannel(string channelId);
    // public Task EnqueuePlaylist(string playlistId);
    public Task<QueuedDownload?> DequeueDownload();

}
