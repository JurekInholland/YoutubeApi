using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Models;
using Models.DomainModels;

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
        await Send("taskUpdate", new TaskProgress
        {
            Time = DateTime.Now,
            Task = task,
            Status = status
        });
    }

    public async Task SendDownloadProgress(DownloadProgress progress)
    {
        await Send("downloadProgress", progress);
    }

    public async Task SendObject(string user, string callbackName, object obj)
    {
        // _logger.LogInformation("Sending object to {User}", user);
        await Send(callbackName, obj);
    }

    public async Task SendLocalVideo(LocalVideo local)
    {
        await Send("localVideo", local);
    }

    private async Task Send(string method, object? obj)
    {
        try
        {
            await Clients.All.SendAsync(method, obj);
        }
        catch (Exception e)
        {
            _logger.LogWarning("Exception during send: {Exception}", e);
        }
    }
}
