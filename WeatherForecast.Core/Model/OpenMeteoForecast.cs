using System.Text.Json.Serialization;

namespace WeatherForecast.Core.Model;

/// <summary>
/// Open Meteo Output format.
/// </summary>
public class OpenMeteoForecast
{
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public Daily Daily { get; set; } = new();
}

public class Daily
{
    public DateTime[] Time { get; set; } = Array.Empty<DateTime>();
    [JsonPropertyName("temperature_2m_min")]
    public float[] MinTemperatures { get; set; } = Array.Empty<float>();
    [JsonPropertyName("temperature_2m_max")]
    public float[] MaxTemperatures { get; set; } = Array.Empty<float>();
    [JsonPropertyName("precipitation_sum")]
    public float[] Precipitations { get; set; } = Array.Empty<float>();
}