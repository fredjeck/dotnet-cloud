using Microsoft.Extensions.Logging;
using Moq;
using WeatherForecast.Api.Controllers;
using FluentAssertions;
using Moq.Protected;
using WeatherForecast.Core.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecast.Tests.Controllers;

public class WeatherForecastControllerTest
{
    [Fact]
    public async Task ItShouldGenerateWeatherForecastAsync()
    {
         // Arrange
        var mockedLocationHandler = new Mock<HttpMessageHandler>();
        var expectedLocationResponse = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("[{\"display_name\":\"Windsor Castle\",\"lat\":51.483757,\"lon\":-0.6040964}]") };
        mockedLocationHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedLocationResponse);
        var httpLocationClient = new HttpClient(mockedLocationHandler.Object);

        var mockedWeahterHandler = new Mock<HttpMessageHandler>();
        var expectedWeatherResponse = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent("{\"latitude\":51.48,\"longitude\":-0.60000014,\"daily\":{\"time\":[\"2022-10-01T00:00:00\",\"2022-10-02T00:00:00\",\"2022-10-03T00:00:00\",\"2022-10-04T00:00:00\",\"2022-10-05T00:00:00\",\"2022-10-06T00:00:00\",\"2022-10-07T00:00:00\"],\"temperature_2m_min\":[11.9,9.8,4.5,9.5,10,9.1,10.9],\"temperature_2m_max\":[18.1,17,16.7,18.3,16.6,17.3,16.7],\"precipitation_sum\":[0,6.6,0,0,2.4,0,3.3]}}") };
        mockedWeahterHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedWeatherResponse);
        var httpWeatherClient = new HttpClient(mockedWeahterHandler.Object);
        
        var controller = new WeatherForecastController(new Mock<ILogger<WeatherForecastController>>().Object
            , new NominatimGeoCodingService(httpLocationClient, new Mock<ILogger<NominatimGeoCodingService>>().Object)
            , new OpenMeteoForecastService(httpWeatherClient, new Mock<ILogger<OpenMeteoForecastService>>().Object));

        //Act
        var response = await controller.Get("Search string");

        //Assert
        response.Should().NotBeNull();
        response.Value?.Forecasts.Should().HaveCount(7);
    }

    [Fact]
    public async Task ItShouldHandleMissingLocation()
    {
         // Arrange
        var mockedLocationHandler = new Mock<HttpMessageHandler>();
        var expectedLocationResponse = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.NotFound, Content = new StringContent("[]") };
        mockedLocationHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedLocationResponse);
        var httpLocationClient = new HttpClient(mockedLocationHandler.Object);

        var nominatimMock = new NominatimGeoCodingService(httpLocationClient, new Mock<ILogger<NominatimGeoCodingService>>().Object);
        var meteoMock = new Mock<OpenMeteoForecastService>(new Mock<HttpClient>().Object, new Mock<ILogger<OpenMeteoForecastService>>().Object);
        
        var controller = new WeatherForecastController(new Mock<ILogger<WeatherForecastController>>().Object
            , nominatimMock
            , meteoMock.Object);

        //Act
        var response = await controller.Get("Search string");

        //Assert
        response.Should().NotBeNull();
        response.Result.Should().BeAssignableTo<BadRequestObjectResult>();
    }
}