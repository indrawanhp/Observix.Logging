using Observix.Logging.Extensions;
using Observix.Logging.Logging;
using Observix.Logging.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseObservix();

// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddObservix(
    builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseObservix();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool",
    "Mild", "Warm", "Balmy", "Hot",
    "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable
        .Range(1, 5)
        .Select(index =>
            new WeatherForecast(
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[
                    Random.Shared.Next(
                        summaries.Length)]
            ))
        .ToArray();

    return forecast;
});

app.MapGet("/error", () =>
{
    throw new Exception(
        "Cherry Test Exception");
});

app.MapGet(
    "/business",
    (ILoggingService logging) =>
    {
        var log =
            logging.Initial(
                serviceLayer:
                ServiceLayer.Api,

                functionLayer:
                FunctionLayer.Business,

                className:
                "BusinessEndpoint",

                methodName:
                "GetBusiness",

                requestBody:
                new
                {
                    Username = "Maverick",
                    Password = "123456"
                });

        logging.Info<Program>(
            logging.Finalize(
                log,
                true,
                "Business Success",
                EventType.Information,
                responseBody:
                new
                {
                    Result = "OK"
                }));

        return Results.Ok(
            new
            {
                Message = "Business Success"
            });
    });

app.Run();
Log.CloseAndFlush();

record WeatherForecast(
    DateOnly Date,
    int TemperatureC,
    string? Summary)
{
    public int TemperatureF =>
        32 + (int)(TemperatureC / 0.5556);
}