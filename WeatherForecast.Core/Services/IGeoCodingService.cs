using WeatherForecast.Core.Model;

namespace WeatherForecast.Core.Services;

/// <summary>
/// Interface defining the behavior for services allowing the geocoding of full text locations.
/// </summary>
public interface IGeoCodingService
{
    /// <summary>
    /// Searches for the provided full text location (i.e. Promenade des Champs-Élysées) and returns geo coded location results.
    /// </summary>
    /// <param name="location">The location to search for</param>
    /// <returns>The matching geo-coded locations or an empty collection</returns>
    Task<IEnumerable<LatLong>> SearchLocation(string location);
}