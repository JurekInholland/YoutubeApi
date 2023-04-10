using Domain.Context;
using Domain.Repositories.ChannelRepo;
using Domain.Repositories.LocalVideoRepo;
using Domain.Repositories.QueuedDownloadRepo;
using Domain.Repositories.SettingsRepo;
using Domain.Repositories.YoutubeVideoRepo;

namespace Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly YoutubeAppContext _context;
    private IYoutubeVideoRepository? _youtubeVideoRepository;
    private IQueuedDownloadRepository? _queuedDownloadRepository;
    private IApplicationSettingsRepository? _applicationSettingsRepository;
    private ILocalVideoRepository? _localVideoRepository;

    private IYoutubeChannelRepository? _youtubeChannelRepository;

    public IYoutubeVideoRepository YoutubeVideos => _youtubeVideoRepository ??= new YoutubeVideoRepository(_context);
    public IQueuedDownloadRepository QueuedDownloads => _queuedDownloadRepository ??= new QueuedDownloadRepository(_context);

    public IApplicationSettingsRepository ApplicationSettings =>
        _applicationSettingsRepository ??= new ApplicationSettingsRepository(_context);

    public ILocalVideoRepository LocalVideos => _localVideoRepository ??= new LocalVideoRepository(_context);

    public IYoutubeChannelRepository YoutubeChannels => _youtubeChannelRepository ??= new YoutubeChannelRepository(_context);

    public UnitOfWork(YoutubeAppContext context)
    {
        _context = context;
    }


    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
