namespace ColorfulClock.Server.Services;

using Microsoft.AspNetCore.SignalR;
using ColorfulClock.Server.Hubs;
using ColorfulClock.Server.Services.Contracts;

public class ColorBackgroundService : BackgroundService
{
    private readonly ILogger<ColorBackgroundService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<ColoredClockHub> _hubContext;
    private readonly IColoredTimeService _coloredTimeService;

    public ColorBackgroundService(
            ILogger<ColorBackgroundService> logger, 
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
            _logger.LogInformation($"ColorBackgroundService running at: {DateTime.Now}\n");

            var color = _coloredTimeService.GetNewColor();
            await _hubContext.Clients.All.SendAsync("ReceiveColor", color);

            bool getCooldown = int.TryParse(_configuration["CooldownForSendingColor"], out int cooldown);
            await Task.Delay(getCooldown ? cooldown : 5000);
        }
    }
}
