using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Observix.Logging.Logging;
using Observix.Logging.Models;
using Observix.Logging.Providers.Serilog;

namespace Observix.Logging.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseObservix(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((_, _) =>
        {
            // placeholder
        });

        hostBuilder.ConfigureServices((context, _) =>
        {
            var configuration =
                context.Configuration;
        
            var options =
                new LoggingOptions();
        
            configuration
                .GetSection(
                    LoggingOptions.SectionName)
                .Bind(options);
            
            var appInfo =
                new AppInfoOptions();
        
            configuration
                .GetSection("AppInfo")
                .Bind(appInfo);
        
            if (options.Provider == ProviderType.Serilog)
            {
                SerilogConfigurator.Configure(
                    configuration,
                    options,
                    appInfo);
            }
        });
        
        return hostBuilder;
    }
}