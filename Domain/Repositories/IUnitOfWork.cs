using Domain.Repositories.QueuedDownload;
using Domain.Repositories.YoutubeVideo;

namespace Domain.Repositories;

public interface IUnitOfWork
    // : IDisposable
{
    IYoutubeVideoRepository YoutubeVideos { get; }
    IQueuedDownloadRepository QueuedDownloads { get; }

    Task Save();
}
