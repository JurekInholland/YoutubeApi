using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.QueueService;

namespace Services.TaskService;

public class TaskService : BackgroundService, ITaskService
{
    private readonly ILogger<TaskService> _logger;

    private PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(100000));
    private readonly IQueueService _queueService;

    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
    }

    public void ChangeInterval(TimeSpan interval)
    {
        _timer = new PeriodicTimer(interval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            await DoWorkAsync();
            // await _queueService.ProcessQueue();
        }
    }

    private async Task DoWorkAsync()
    {
        _logger.LogInformation(DateTime.Now.ToString("O"));
        await Task.Delay(2000);
    }
}
