using System.Net.Http.Json;
using WeatherForecast.Core.Model;
using Microsoft.Extensions.Logging;

namespace WeatherForecast.Core.Services.Impl;

/// <summary>
/// A GeoCoding Implementation which uses OSM Nominatim to search for locations.
/// </summary>
/// <seelalso href="https://nominatim.org/release-docs/develop/api/Search/">The nominatim official reference</seelalso>
public class OpenMeteoForecastService
 : IWeatherForecastService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenMeteoForecastService> _logger;

    public OpenMeteoForecastService(HttpClient httpClient, ILogger<OpenMeteoForecastService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.open-meteo.com"); // In a real life application this shall be handled in the appsettings
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "WeatherForecastService");
    }

    /// <inheritdoc/>
    public async Task<WeatherReport?> ForecastForLocation(LatLong location)
    {
        // For the sake of demo, this method will just throw exceptions on failures
        var result = await _httpClient.GetFromJsonAsync<OpenMeteoForecast>($"/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=temperature_2m_max,temperature_2m_min,sunrise,sunset,precipitation_sum&timezone=Europe%2FBerlin");
        if (result is not null)
        {
            return new WeatherReport()
            {
                LatLong = location,
                Forecasts = result.Daily.Time.Select((t, i) => new DailyWeather()
                {
                    Date = t,
                    MinTemperature = result.Daily.MinTemperatures[i],
                    MaxTemperature = result.Daily.MaxTemperatures[i],
                    Precipitations = result.Daily.Precipitations[i]
                })

            };

        }

        return null;
    }
}