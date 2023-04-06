using Microsoft.Extensions.Hosting;

namespace Services.BackgroundServices;

public class BackgroundTaskService : BackgroundService
{
    private readonly IBackgroundTaskQueue _taskQueue;

    public BackgroundTaskService(IBackgroundTaskQueue taskQueue)
    {
        _taskQueue = taskQueue;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _taskQueue.DequeueAsync(stoppingToken);
            if (workItem is null)
                continue;
            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BackgroundTaskService Exception: " + ex.Message);
                // Handle the exception or log it as needed.
            }
        }
    }
}
