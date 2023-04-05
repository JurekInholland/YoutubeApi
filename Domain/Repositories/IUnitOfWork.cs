﻿using Domain.Repositories.LocalVideoRepo;
using Domain.Repositories.QueuedDownloadRepo;
using Domain.Repositories.SettingsRepo;
using Domain.Repositories.YoutubeVideo;

namespace Domain.Repositories;

public interface IUnitOfWork
    // : IDisposable
{
    IYoutubeVideoRepository YoutubeVideos { get; }
    IQueuedDownloadRepository QueuedDownloads { get; }
    IApplicationSettingsRepository ApplicationSettings { get; }
    ILocalVideoRepository LocalVideos { get; }
    Task Save();
}
