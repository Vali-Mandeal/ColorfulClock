namespace ColorfulClock.Server.Services;

using ColorfulClock.Server.Models;
using ColorfulClock.Server.Services.Contracts;
using System.Text.Json;

public class ColoredTimeService :  IColoredTimeService
{
    private List<Color> _colors = new List<Color>();
    private readonly ILogger<ColoredTimeService> _logger;

    public ColoredTimeService(ILogger<ColoredTimeService> logger)
    {
        _logger = logger;
    }

    public async Task<Color> GetNewColor()
    {
        return await GetRandomColor();
    }

    public string GetFormattedTime()
    {
        return DateTime.Now.ToString("HH:mm:ss");
    }

    private async Task<Color> GetRandomColor()
    {
        await SetColors();
        var randomIndex = GetRandomNumber(0, _colors.Count);

        return _colors[randomIndex];
    }

    private async Task SetColors()
    {
        try
        {
            var jsonString = await File.ReadAllTextAsync("./Properties/colors.json");

            _colors = JsonSerializer.Deserialize<List<Color>>(jsonString);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Cannot deserialize colors.json: {ex}");
            throw;
        }
    }

    private int GetRandomNumber(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
}
