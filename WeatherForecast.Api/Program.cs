using Polly;
using Polly.Extensions.Http;
using WeatherForecast.Core.Services;
using WeatherForecast.Core.Services.Impl;

var builder = ConfigureBuilder(WebApplication.CreateBuilder(args));
ConfigureApplication(builder.Build()).Run();


/// <summary>
/// Configure the WebApplicationBuilder
/// </summary>
static WebApplicationBuilder ConfigureBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddHttpClient<IGeoCodingService, NominatimGeoCodingService>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
    builder.Services.AddHttpClient<IWeatherForecastService, OpenMeteoForecastService>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    return builder;
}

/// <summary>
/// Configure the WebApplication
/// </summary>
static WebApplication ConfigureApplication(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    return app;
}

/// <summary>
/// Uses Polly to define a Circuit Breaker policu which will break after 5 events for a duration of 30 seconds
/// </summary>
/// <returns>The circuit breaker policy</returns>
static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

/// <summary>
/// Uses Polly to define a retry policy in case the remote endpoint is not responding. Will try 3 times every 2 seconds exponentially
/// </summary>
/// <returns>The retry breaker policy</returns>
static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound).WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));