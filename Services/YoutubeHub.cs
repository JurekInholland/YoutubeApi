using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Services;

public class YoutubeHub : Hub
{
    private readonly ILogger<YoutubeHub> _logger;

    public YoutubeHub(ILogger<YoutubeHub> logger)
    {
        _logger = logger;
    }

    public string GetConnectionId() => Context.ConnectionId;

    public async Task SendObjects(string user, string callbackName, object obj)
    {
        // _logger.LogInformation("Sending object to {User}", user);
        await Clients.All.SendAsync(callbackName, obj);
    }
}
