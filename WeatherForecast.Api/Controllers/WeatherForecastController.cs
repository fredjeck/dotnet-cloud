using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Model;
using WeatherForecast.Core.Services;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IGeoCodingService _geoCodingService;
    private IWeatherForecastService _weatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IGeoCodingService geoService, IWeatherForecastService weatherService)
    {
        _logger = logger;
        _geoCodingService = geoService;
        _weatherService = weatherService;
    }

    /// <summary>
    /// Provides a 7 days weather forecast for the supplied location.
    /// </summary>
    /// <param name="q">The queried full-text location</param>
    /// <returns>A weeather forecast</returns>
    /// <response code="200">When the location is found and a forecast can be generated for the location</response>
    /// <response code="400">When the location does not exist or when the report generation fails</response>
    [HttpGet(Name = "GetWeatherForecast")]
    [ProducesResponseType(typeof(WeatherReport), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<ActionResult<WeatherReport>> Get(string q)
    {
        var loc = await _geoCodingService.SearchLocation(q);
        if (!loc.Any())
        {
            return BadRequest("Location not found");
        }

        WeatherReport? report = default;
        try
        {
            report = await _weatherService.ForecastForLocation(loc.First());
        }
        catch { }

        return report is null ? BadRequest("No report found for location") : Ok(report);
    }
}
