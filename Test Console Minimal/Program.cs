using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Observix.Logging.Extensions;
using Observix.Logging.Logging;
using Observix.Logging.Models;

var builder =
    Host.CreateDefaultBuilder(args);

builder.UseObservix();

builder.ConfigureServices((context, services) =>
{
    services.AddObservix(
        context.Configuration);
});

var host = builder.Build();

var logger =
    host.Services.GetRequiredService<
        ILoggingService>();

var log =
    logger.Initial(
        serviceLayer: ServiceLayer.Api,
        functionLayer: FunctionLayer.Business,
        className: "Program",
        methodName: "Main",
        requestBody: new
        {
            Username = "Maverick",
            Password = "123456"
        });

await Task.Delay(500);

logger.Info<Program>(
    logger.Finalize(
        log,
        true,
        "Success",
        EventType.Information));