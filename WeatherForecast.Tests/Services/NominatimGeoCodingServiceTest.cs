using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherForecast.Api.Controllers;
using FluentAssertions;
using WeatherForecast.Core.Services.Impl;

namespace WeatherForecast.Tests.Controllers;

public class NominatimGeoCodingServiceTest : UnitTestBase
{
    [Fact]
    public async Task ItShouldReturnLocations()
    {
        // Arrange
        var service = PrepareGeoCodingService(System.Net.HttpStatusCode.OK, "[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]");

        //Act
        var locations = await service.SearchLocation("locations");

        //Assert
        locations.Should().NotBeEmpty();
        locations.Should().HaveCount(1);
        Assert.Equal("Windsor Castle", locations.First().DisplayName);
    }

     [Fact]
    public async Task ItShouldHandleHttpFailures()
    {
        // Arrange
        var service = PrepareGeoCodingService(System.Net.HttpStatusCode.BadGateway, "[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]");

        //Act
        var locations = await service.SearchLocation("locations");

        //Assert
        locations.Should().BeEmpty();
    }
}