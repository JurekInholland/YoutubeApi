namespace Domain.Repositories.QueuedDownload;

public interface IQueuedDownloadRepository : IRepositoryBase<Models.DomainModels.QueuedDownload>
{
    public Task Enqueue(Models.DomainModels.QueuedDownload queuedDownload);

}
