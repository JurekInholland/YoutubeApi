using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Models;

namespace Services;

public class YoutubeHub : Hub
{
    private readonly ILogger<YoutubeHub> _logger;

    public YoutubeHub(ILogger<YoutubeHub> logger)
    {
        _logger = logger;
    }

    public string GetConnectionId() => Context.ConnectionId;

    public async Task SendTaskUpdate(Enums.ApplicationTask task, Enums.TaskStatus status)
    {
        await Clients.All.SendAsync("taskUpdate", new TaskProgress
        {
            Time = DateTime.Now,
            Task = task,
            Status = status
        });
    }

    public async Task SendDownloadProgress(DownloadProgress progress)
    {
        await Clients.All.SendAsync("downloadProgress", progress);
    }

    public async Task SendObject(string user, string callbackName, object obj)
    {
        // _logger.LogInformation("Sending object to {User}", user);
        await Clients.All.SendAsync(callbackName, obj);
    }
}
