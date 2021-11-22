namespace ColorfulClock.Server.Hubs;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ColoredClockHub : Hub
{
    private readonly ILogger _logger;

    private static List<string> _connectedUsers = new();

    public ColoredClockHub(ILogger<ColoredClockHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _connectedUsers.Add(Context.ConnectionId);

        _logger.LogInformation($"User {Context.ConnectionId} connected. Active connections: {_connectedUsers.Count}");

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectedUsers.Remove(Context.ConnectionId);

        _logger.LogInformation($"User {Context.ConnectionId} disconnected. Remaining active connections: {_connectedUsers.Count}");

        return base.OnDisconnectedAsync(exception);
    }
}
