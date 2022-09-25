using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Services;
using WeatherForecast.Core.Model;

namespace WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{
    private readonly IGeoCodingService _geoCodingService;

    private readonly ILogger<LocationController> _logger;

    public LocationController(ILogger<LocationController> logger, IGeoCodingService service)
    {
        _logger = logger;
        _geoCodingService = service;
    }

    [HttpGet(Name = "GetLocation")]
    public async Task<IEnumerable<GeoCodingResult>> Get(string q)
    {
        return await _geoCodingService.SearchLocation(q);
    }
}
