using Models;
using Models.DomainModels;

namespace Domain.Repositories.QueuedDownloadRepo;

public interface IQueuedDownloadRepository : IRepositoryBase<QueuedDownload>
{
    // public Task Enqueue(Models.DomainModels.QueuedDownload queuedDownload);
    public Task<IEnumerable<QueuedDownload>> GetByStatus(Enums.DownloadStatus status);

    public Task DeleteByStatus(Enums.DownloadStatus status);

    public Task EnqueueDownload(QueuedDownload queuedDownload);
}
