using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherForecast.Api.Controllers;
using FluentAssertions;
using WeatherForecast.Core.Services.Impl;

namespace WeatherForecast.Tests.Controllers;

public class LocationControllerTest
{
    [Fact]
    public async Task ItShouldReturnLocations()
    {
         // Arrange
        var mockedHandler = new Mock<HttpMessageHandler>();
        var expectedResponse = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]") };
        mockedHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);
        var httpClient = new HttpClient(mockedHandler.Object);
        
        var controller = new LocationController(new Mock<ILogger<LocationController>>().Object, new NominatimGeoCodingService(httpClient, new Mock<ILogger<NominatimGeoCodingService>>().Object));

        //Act
        var response = await controller.Get("Search string");

        //Assert
        response.Should().NotBeEmpty();
        response.Should().HaveCount(1);
        Assert.Equal("Windsor Castle", response.First().DisplayName);
    }
}