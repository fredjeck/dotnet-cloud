using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherForecast.Api.Controllers;
using FluentAssertions;
using WeatherForecast.Core.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Core.Model;

namespace WeatherForecast.Tests.Controllers;

public class LocationControllerTest : UnitTestBase
{
    [Fact]
    public async Task ItShouldReturnLocations()
    {
        // Arrange
        var controller = new LocationController(new Mock<ILogger<LocationController>>().Object, PrepareGeoCodingService(System.Net.HttpStatusCode.OK, "[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]"));

        //Act
        var response = await controller.Get("Windsor");

        //Assert
        response.Should().NotBeNull();
        response.Result.Should().BeAssignableTo<OkObjectResult>();

        var result = response.Result as OkObjectResult;
        var locations = result?.Value as IEnumerable<LatLong>;

        locations.Should().NotBeNull();
        locations.Should().HaveCount(1);
    }

    [Fact]
    public async Task ItShouldBounceMissingQueryString()
    {
        // Arrange
        var controller = new LocationController(new Mock<ILogger<LocationController>>().Object, PrepareGeoCodingService(System.Net.HttpStatusCode.OK, "[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]"));

        //Act
        var response = await controller.Get(" ");

        //Assert
        response.Should().NotBeNull();
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
    }
}