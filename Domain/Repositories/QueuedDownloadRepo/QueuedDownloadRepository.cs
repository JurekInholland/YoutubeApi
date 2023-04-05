using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DomainModels;

namespace Domain.Repositories.QueuedDownloadRepo;

public class QueuedDownloadRepository : RepositoryBase<QueuedDownload>, IQueuedDownloadRepository
{
    public QueuedDownloadRepository(DbContext context) : base(context)
    {
        Context = context;
    }

    public async Task DeleteByStatus(Enums.DownloadStatus status)
    {
        Context.Set<QueuedDownload>().RemoveRange(await YoutubeContext.QueuedDownloads.Where(d => d.Status == status).ToListAsync());
    }

    // public async Task Enqueue(Models.DomainModels.QueuedDownload queuedDownload)
    // {
    //     if (await YoutubeContext.YoutubeVideos.FindAsync(queuedDownload.Video.Id) == null)
    //     {
    //         YoutubeContext.Entry(queuedDownload.Video).State = EntityState.Added;
    //     }
    //     else
    //     {
    //         YoutubeContext.Entry(queuedDownload.Video).State = EntityState.Detached;
    //     }
    //
    //     YoutubeContext.QueuedDownloads.Attach(queuedDownload);
    //     YoutubeContext.Entry(queuedDownload).State = EntityState.Added;
    //     await YoutubeContext.SaveChangesAsync();
    // }

    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;

    public async Task<IEnumerable<QueuedDownload>> GetByStatus(Enums.DownloadStatus status)
    {
        return await YoutubeContext.QueuedDownloads.Where(d => d.Status == status).ToListAsync();
    }
}
