using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WeatherForecast.Core.Services;
using WeatherForecast.Core.Services.Impl;

namespace WeatherForecast.Tests;

public abstract class UnitTestBase
{

    public IGeoCodingService PrepareGeoCodingService(HttpStatusCode statusCode, string content)
    {
        var mockedHandler = new Mock<HttpMessageHandler>();
        var expectedResponse = new HttpResponseMessage() { StatusCode = statusCode, Content = new StringContent(content) };
        mockedHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);
        var httpClient = new HttpClient(mockedHandler.Object);

        return new NominatimGeoCodingService(httpClient, new Mock<ILogger<NominatimGeoCodingService>>().Object);
    }

    public IWeatherForecastService PrepareWeatherForecastService(HttpStatusCode statusCode, string content)
    {
        var mockedHandler = new Mock<HttpMessageHandler>();
        var expectedResponse = new HttpResponseMessage() { StatusCode = statusCode, Content = new StringContent(content) };
        mockedHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);
        var httpClient = new HttpClient(mockedHandler.Object);

        return new OpenMeteoForecastService(httpClient, new Mock<ILogger<OpenMeteoForecastService>>().Object);
    }
}