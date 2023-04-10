using Domain.Repositories.ChannelRepo;
using Domain.Repositories.LocalVideoRepo;
using Domain.Repositories.QueuedDownloadRepo;
using Domain.Repositories.SettingsRepo;
using Domain.Repositories.YoutubeVideoRepo;

namespace Domain.Repositories;

public interface IUnitOfWork
    // : IDisposable
{
    IYoutubeVideoRepository YoutubeVideos { get; }
    IQueuedDownloadRepository QueuedDownloads { get; }
    IApplicationSettingsRepository ApplicationSettings { get; }
    ILocalVideoRepository LocalVideos { get; }
    IYoutubeChannelRepository YoutubeChannels { get; }

    Task Save();
}
