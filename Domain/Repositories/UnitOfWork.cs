using Domain.Context;
using Domain.Repositories.QueuedDownload;
using Domain.Repositories.YoutubeVideo;

namespace Domain.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly YoutubeAppContext _context;
    private IYoutubeVideoRepository? _youtubeVideoRepository;
    private IQueuedDownloadRepository? _queuedDownloadRepository;

    public IYoutubeVideoRepository YoutubeVideos => _youtubeVideoRepository ??= new YoutubeVideoRepository(_context);
    public IQueuedDownloadRepository QueuedDownloads => _queuedDownloadRepository ??= new QueuedDownloadRepository(_context);

    public UnitOfWork(YoutubeAppContext context)
    {
        _context = context;
    }



    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}
