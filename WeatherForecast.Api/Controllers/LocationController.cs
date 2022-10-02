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
    public async Task<ActionResult<IEnumerable<LatLong>>> Get(string q)
    {
        if(string.IsNullOrWhiteSpace(q)){
            return BadRequest("Please provide a location using the 'q' query parameter");
        }

        // SearchLocation is safe and returns an empty collection if something goes wrong.
        var latlongs = await _geoCodingService.SearchLocation(q);
        return Ok(latlongs);
    }
}
