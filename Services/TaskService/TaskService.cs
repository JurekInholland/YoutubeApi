using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.QueueService;

namespace Services.TaskService;

public class TaskService : BackgroundService, ITaskService
{
    private readonly ILogger<TaskService> _logger;

    private PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(50000));
    // private readonly IQueueService _queueService;

    public TaskService(ILogger<TaskService> logger, IQueueService queueService)
    {
        _logger = logger;
        // _queueService = queueService;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("TaskService started");
        await base.StartAsync(cancellationToken);
    }

    public void ChangeInterval(TimeSpan interval)
    {
        Console.WriteLine($"Changing interval {_timer} => {interval}");
        _timer = new PeriodicTimer(interval);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWorkAsync(stoppingToken);
            // await _queueService.ProcessQueue();
        }
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(DateTime.Now.ToString("O"));

        // await _queueService.ProcessQueue(stoppingToken);
        // await Task.Delay(2000);
    }
}
