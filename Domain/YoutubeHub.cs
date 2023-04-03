using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Domain;

public class YoutubeHub : Hub
{
    private readonly ILogger<YoutubeHub> _logger;

    public YoutubeHub(ILogger<YoutubeHub> logger)
    {
        _logger = logger;
    }

    public string GetConnectionId() => Context.ConnectionId;

    public async Task SendObject(string user, string callbackName, object obj)
    {
        _logger.LogInformation("Sending object to {User}", user);
        await Clients.Client(user).SendAsync(callbackName, obj);
    }
}
