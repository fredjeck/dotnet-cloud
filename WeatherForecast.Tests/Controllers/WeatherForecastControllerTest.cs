using Microsoft.Extensions.Logging;
using Moq;
using WeatherForecast.Api.Controllers;
using FluentAssertions;

namespace WeatherForecast.Tests.Controllers;

public class WeatherForecastControllerTest
{
    [Fact]
    public void ItShouldGenerateWeatherForecast()
    {
        // Arrange
        var logger = new Mock<ILogger<WeatherForecastController>>();
        var controller = new WeatherForecastController(logger.Object);

        //Act
        var response = controller.Get();

        //Assert
        response.Should().NotBeEmpty();
        response.Should().HaveCount(5);
        response.Should().OnlyHaveUniqueItems();
        response.Should().OnlyContain(x => x.TemperatureC >= -20 && x.TemperatureC <= 55);
    }
}