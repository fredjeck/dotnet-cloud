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


    /// <summary>
    /// Searches for a location and returns its latitude and longitude.
    /// </summary>
    /// <param name="q">The queried full-text location</param>
    /// <returns>A list of matching places with their respective coordinates</returns>
    /// <response code="200">When the location is found </response>
    /// <response code="400">When the endpoint is being called with missing parameters</response>
    [ProducesResponseType(typeof(IEnumerable<LatLong>), 200)]
    [ProducesResponseType(typeof(string), 400)]
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
