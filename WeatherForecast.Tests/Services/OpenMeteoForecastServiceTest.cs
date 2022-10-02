using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherForecast.Api.Controllers;
using FluentAssertions;
using WeatherForecast.Core.Services.Impl;
using WeatherForecast.Core.Model;

namespace WeatherForecast.Tests.Controllers;

public class OpenMeteoForecastServiceTest : UnitTestBase
{
    [Fact]
    public async Task ItShouldReturnForecasts()
    {
        // Arrange
        var service = PrepareWeatherForecastService(System.Net.HttpStatusCode.OK, "{\"latitude\":51.48,\"longitude\":-0.60000014,\"daily\":{\"time\":[\"2022-10-01T00:00:00\",\"2022-10-02T00:00:00\",\"2022-10-03T00:00:00\",\"2022-10-04T00:00:00\",\"2022-10-05T00:00:00\",\"2022-10-06T00:00:00\",\"2022-10-07T00:00:00\"],\"temperature_2m_min\":[11.9,9.8,4.5,9.5,10,9.1,10.9],\"temperature_2m_max\":[18.1,17,16.7,18.3,16.6,17.3,16.7],\"precipitation_sum\":[0,6.6,0,0,2.4,0,3.3]}}");

        //Act
        var forecast = await service.ForecastForLocation(new LatLong() { Latitude = 51.48, Longitude = -0.60000014, DisplayName = "Windsor Castle" });

        //Assert
        forecast.Should().NotBeNull();
        forecast?.Forecasts.Should().HaveCount(7);
    }

    [Fact]
    public void ItShouldHandleHttpFailures()
    {
        // Arrange
        var service = PrepareWeatherForecastService(System.Net.HttpStatusCode.BadGateway, "");
        var action = async () => await service.ForecastForLocation(new LatLong() { Latitude = 51.48, Longitude = -0.60000014, DisplayName = "Windsor Castle" });

        //Act
        var ex = Record.ExceptionAsync(action);

        //Assert
        Assert.NotNull(ex);
    }
}