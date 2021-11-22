namespace ColorfulClock.Server.Services;

using Microsoft.AspNetCore.SignalR;
using ColorfulClock.Server.Hubs;
using ColorfulClock.Server.Services.Contracts;

public class TimeBackgroundService : BackgroundService
{
    private readonly ILogger<TimeBackgroundService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<ColoredClockHub> _hubContext;
    private readonly IColoredTimeService _coloredTimeService;

    public TimeBackgroundService(
            ILogger<TimeBackgroundService> logger,
            IConfiguration configuration, 
            IHubContext<ColoredClockHub> hubContext, 
            IColoredTimeService coloredTimeService
        )
    {
        _logger = logger;   
        _configuration = configuration;
        _hubContext = hubContext;
        _coloredTimeService = coloredTimeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"TimeBackgroundService running at: {DateTime.Now}\n");
            var time = _coloredTimeService.GetFormattedTime();

            await _hubContext.Clients.All.SendAsync("ReceiveTime", time);

            bool getCooldown = int.TryParse(_configuration["CooldownForSendingTime"], out int cooldown);
            await Task.Delay(getCooldown ? cooldown : 5000);
        }
    }
}
