using WeatherForecast.Core.Model;

namespace WeatherForecast.Core.Services;

/// <summary>
/// Interface defining the behavior for services allowing to get a weather report out of a latitude and a longitude.
/// </summary>
public interface IWeatherForecastService
{
    /// <summary>
    /// Gets a weather forecast for the provided location
    /// </summary>
    /// <param name="location">The geo point to look for a weather report</param>
    /// <returns>A weather report or null if no data is available</returns>
    Task<WeatherReport?> ForecastForLocation(LatLong location);
}