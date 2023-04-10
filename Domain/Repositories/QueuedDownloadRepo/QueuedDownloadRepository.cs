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

    public async Task EnqueueDownload(QueuedDownload queuedDownload)
    {
        YoutubeChannel? channel =
            await YoutubeContext.YoutubeChannels.FirstOrDefaultAsync(x => x.Id == queuedDownload.Video.YoutubeChannel.Id);

        if (channel is not null)
        {
            Console.WriteLine("channel found");
            queuedDownload.Video.YoutubeChannel = channel;
            // YoutubeContext.Entry(queuedDownload.Video.YoutubeChannel).State = EntityState.Detached;
            // YoutubeContext.Entry(channel).CurrentValues.SetValues(queuedDownload.Video.YoutubeChannel);
        }
        else
        {
            Console.WriteLine("channel not found");
            channel = queuedDownload.Video.YoutubeChannel;
        }

        YoutubeVideo? video = await YoutubeContext.YoutubeVideos.FindAsync(queuedDownload.Video.Id);

        if (video is not null)
        {
            YoutubeContext.Entry(video).CurrentValues.SetValues(queuedDownload.Video);
        }
        else
        {
            channel.Videos ??= new List<YoutubeVideo>();
            channel.Videos.Add(queuedDownload.Video);
        }

        queuedDownload.Video = video ?? queuedDownload.Video;

        QueuedDownload? qD = await YoutubeContext.QueuedDownloads.FindAsync(queuedDownload.Id);
        if (qD is not null)
        {
            Console.WriteLine("qd is not null");
            YoutubeContext.Entry(qD).CurrentValues.SetValues(queuedDownload);
        }
        else
        {
            Console.WriteLine("creating queued downlaod");
            // YoutubeContext.QueuedDownloads.Add(queuedDownload);
            // await Create(queuedDownload);
            await YoutubeContext.Set<QueuedDownload>().AddAsync(queuedDownload);
            await YoutubeContext.SaveChangesAsync();
        }
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
