
using Microsoft.AspNetCore.SignalR;

namespace App;

/// <summary>
/// SignalR hub
/// </summary>
public class YoutubeHub : Hub
{

    /// <summary>
    /// Send an object to a client
    /// </summary>
    /// <param name="user"></param>
    /// <param name="callbackName"></param>
    /// <param name="obj"></param>
    public async Task SendObject(string user, string callbackName, object obj)
    {
        await Clients.Client(user).SendAsync(callbackName, obj);
    }
}
