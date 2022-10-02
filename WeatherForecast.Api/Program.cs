using System.Reflection;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using WeatherForecast.Core.Services;
using WeatherForecast.Core.Services.Impl;

var builder = ConfigureBuilder(WebApplication.CreateBuilder(args));
ConfigureApplication(builder.Build()).Run();


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
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Weather Report API",
                Version = "v1",
                Description ="Provides accurate weather report"
            }
        );

        var location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        foreach (var filePath in Directory.GetFiles(location ?? ".", "*.xml"))
        {
            c.IncludeXmlComments(filePath);
        }
    });
    return builder;
}

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

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() => HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() => HttpPolicyExtensions.HandleTransientHttpError().OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound).WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));