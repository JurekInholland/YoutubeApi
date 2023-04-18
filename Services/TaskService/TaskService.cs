using System.Diagnostics;
using Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;

namespace Services.TaskService;

public class TaskService : BackgroundService, ITaskService
{
    private readonly ILogger<TaskService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    private PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000));
    private PeriodicTimer _cleaningTaskTimer = new(TimeSpan.FromMilliseconds(5000));


    public TaskService(ILogger<TaskService> logger, IServiceScopeFactory scopeFactory)

    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Entrypoint
    /// </summary>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var unitOfWork = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();
        ApplicationSettings settings = await unitOfWork.ApplicationSettings.GetSettings();
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
        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        Stopwatch sw = new();
        sw.Start();
        var unitOfWork = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();

        ApplicationSettings settings = await unitOfWork.ApplicationSettings.GetSettings();
        _logger.LogInformation("Doing cleanup {Duration} in {Time}", settings.MaxVideoDuration, sw.ElapsedTicks);
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested) return;

        _logger.LogInformation("DT: {Date}", DateTime.Now.ToString("O"));
    }
}
