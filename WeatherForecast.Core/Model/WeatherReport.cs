using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Model;

/// <summary>
/// An improved output format for our weather reports.
/// </summary>
public class WeatherReport
{
    public LatLong LatLong { get; init; } = new();
    public IEnumerable<DailyWeather> Forecasts { get; init; } = Enumerable.Empty<DailyWeather>();
}

public class DailyWeather
{
    public DateTime Date { get; init; }
    public float MinTemperature { get; init; }
    public float MaxTemperature { get; init; }
    public float Precipitations { get; init; }
}
