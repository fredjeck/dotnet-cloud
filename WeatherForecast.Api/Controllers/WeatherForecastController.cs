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

    [HttpGet(Name = "GetWeatherForecast")]
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
