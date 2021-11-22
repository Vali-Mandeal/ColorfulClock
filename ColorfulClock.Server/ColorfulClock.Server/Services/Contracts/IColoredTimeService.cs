namespace ColorfulClock.Server.Services.Contracts;

using ColorfulClock.Server.Models;

public interface IColoredTimeService
{
    Task<Color> GetNewColor();
    string GetFormattedTime();
}
