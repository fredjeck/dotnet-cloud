using System.Net.Http.Json;
using WeatherForecast.Core.Model;
using Microsoft.Extensions.Logging;

namespace WeatherForecast.Core.Services.Impl;

/// <summary>
/// A GeoCoding Implementation which uses OSM Nominatim to search for locations.
/// </summary>
/// <seelalso href="https://nominatim.org/release-docs/develop/api/Search/">The nominatim official reference</seelalso>
public class NominatimGeoCodingService : IGeoCodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NominatimGeoCodingService> _logger;

    public NominatimGeoCodingService(HttpClient httpClient, ILogger<NominatimGeoCodingService> logger)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://nominatim.openstreetmap.org"); // In a real life application this shall be handled in the appsettings
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "NominatimGeoCodingService");
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<LatLong>> SearchLocation(string location)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<LatLong[]>($"/search?q={location}&format=json&addressdetails=1") ?? Enumerable.Empty<LatLong>();
        }
        catch
        {
            return Enumerable.Empty<LatLong>();
        }
    }
}