namespace Services.BackgroundServices;

public interface IBackgroundTaskQueue
{
    void QueueBackgroundWorkItem(Func<CancellationToken, ValueTask> workItem);

    Task<Func<CancellationToken, ValueTask>?> DequeueAsync(CancellationToken cancellationToken);
}
