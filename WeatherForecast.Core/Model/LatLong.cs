using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Model;

/// <summary>
/// A shortened version of the Nominatim GeoCoding search result
/// <seelalso href="https://nominatim.org/release-docs/develop/api/Search/">The nominatim official reference</remark>
/// </summary>
public class LatLong
{
    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = "";
    [JsonPropertyName("lat")]
    public double Latitude { get; set; }
    [JsonPropertyName("lon")]
    public double Longitude { get; set; }
}