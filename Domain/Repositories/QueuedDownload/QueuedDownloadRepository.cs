using Domain.Repositories.YoutubeVideo;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.QueuedDownload;

public class QueuedDownloadRepository : RepositoryBase<Models.QueuedDownload>, IQueuedDownloadRepository
{
    public QueuedDownloadRepository(DbContext context) : base(context)
    {
        Context = context;
    }

}
