using System.Diagnostics;
using Domain.Migrations;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Services.QueueService;

namespace Services.TaskService;

public class TaskService : BackgroundService, ITaskService
{
    private readonly ILogger<TaskService> _logger;

    private PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000));


    private PeriodicTimer _cleaningTaskTimer = new(TimeSpan.FromMilliseconds(5000));

    private IUnitOfWork _unitOfWork;

    private readonly IQueueService _queueService;

    private readonly IServiceScopeFactory _scopeFactory;
    // private readonly IQueueService _queueService;

    public TaskService(ILogger<TaskService> logger, IServiceScopeFactory scopeFactory)

    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        var scope = scopeFactory.CreateScope();
        // _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _queueService = scope.ServiceProvider.GetRequiredService<IQueueService>();
        // _queueService = queueService;
    }

    /// <summary>
    /// Entrypoint
    /// </summary>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {

        _unitOfWork = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();
        ApplicationSettings settings = await _unitOfWork.ApplicationSettings.GetSettings();
        _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(settings.WorkInterval));
        _cleaningTaskTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(settings.CleanUpInterval));

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
        var tasks = new List<Task>
        {
            AwaitWork(stoppingToken),
            AwaitCleanup(stoppingToken)
        };
        await Task.WhenAll(tasks);
    }

    private async Task AwaitWork(CancellationToken stoppingToken)
    {
        while (await _timer.WaitForNextTickAsync(stoppingToken))
        {
            await DoWorkAsync(stoppingToken);
            // await _queueService.ProcessQueue();
        }
    }

    private async Task AwaitCleanup(CancellationToken stoppingToken)
    {
        while (await _cleaningTaskTimer.WaitForNextTickAsync(stoppingToken))
        {
            await DoCleanup(stoppingToken);
        }
    }

    private async Task DoCleanup(CancellationToken stoppingToken)
    {
        Stopwatch sw = new();
        sw.Start();
        _unitOfWork = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        ApplicationSettings settings = await _unitOfWork.ApplicationSettings.GetSettings();
        _logger.LogInformation("Doing cleanup {Duration} in {Time}", settings.MaxVideoDuration, sw.ElapsedTicks);
        // await _queueService.ClearQueue();
        // await Task.Delay(2000, stoppingToken);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DT: {Date}", DateTime.Now.ToString("O"));
        // await Task.Delay(2000, stoppingToken);

        // await _queueService.ProcessQueue(stoppingToken);
        // await Task.Delay(2000);
    }
}
