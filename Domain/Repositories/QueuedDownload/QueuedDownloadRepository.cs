using Domain.Context;
using Domain.Repositories.YoutubeVideo;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.QueuedDownload;

public class QueuedDownloadRepository : RepositoryBase<Models.DomainModels.QueuedDownload>, IQueuedDownloadRepository
{
    public QueuedDownloadRepository(DbContext context) : base(context)
    {
        Context = context;
    }

    public async Task Enqueue(Models.DomainModels.QueuedDownload queuedDownload)
    {
        if (await YoutubeContext.YoutubeVideos.FindAsync(queuedDownload.Video.Id) == null)
        {
            YoutubeContext.Entry(queuedDownload.Video).State = EntityState.Added;
        }
        else
        {
            YoutubeContext.Entry(queuedDownload.Video).State = EntityState.Detached;
        }

        YoutubeContext.QueuedDownloads.Attach(queuedDownload);
        YoutubeContext.Entry(queuedDownload).State = EntityState.Added;
        await YoutubeContext.SaveChangesAsync();
    }

    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;
}
