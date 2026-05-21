using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Observix.Logging.Context;
using Observix.Logging.Logging;
using Observix.Logging.Models;
using Observix.Logging.Providers;
using Observix.Logging.Providers.Console;
using Observix.Logging.Providers.Serilog;
using Observix.Logging.Serialization;

namespace Observix.Logging.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddObservix(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options =
            new LoggingOptions();

        var appInfo =
            new AppInfoOptions();

        configuration
            .GetSection(
                AppInfoOptions.SectionName)
            .Bind(appInfo);

        services.AddSingleton(
            appInfo);

        configuration
            .GetSection(
                LoggingOptions.SectionName)
            .Bind(options);

        services.AddSingleton(
            options);

        services.AddSingleton<
            ILogContextAccessor,
            DefaultLogContextAccessor>();

        services.AddSingleton<
            ILogSerializer,
            MaskedLogSerializer>();

        services.AddSingleton(
            new MaskingOptions());

        switch (options.Provider)
        {
            case ProviderType.Console:

                services.AddSingleton<
                    ILoggingProvider,
                    ConsoleLoggingProvider>();

                break;

            case ProviderType.Serilog:

                services.AddSingleton<
                    ILoggingProvider,
                    SerilogLoggingProvider>();

                break;

            default:

                services.AddSingleton<
                    ILoggingProvider,
                    ConsoleLoggingProvider>();

                break;
        }

        services.AddSingleton<
            ILoggingService,
            LoggingService>();

        return services;
    }
}