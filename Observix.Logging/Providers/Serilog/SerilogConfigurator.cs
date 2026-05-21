using Microsoft.Extensions.Configuration;
using Observix.Logging.Models;
using Observix.Logging.Providers.Serilog.Enrichers;
using Observix.Logging.Providers.Serilog.Helpers;
using Serilog;
using Serilog.Events;

namespace Observix.Logging.Providers.Serilog;

public static class SerilogConfigurator
{
    public static void Configure(
        IConfiguration configuration,
        LoggingOptions options,
        AppInfoOptions appInfo)
    {
        Log.CloseAndFlush();
        
        var loggerConfiguration =
            new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override(
                    "Microsoft",
                    LogEventLevel.Warning)
                .MinimumLevel.Override(
                    "System",
                    LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.With<TraceIdEnricher>()
                .Enrich.WithProperty("EnvironmentName", options.EnvironmentName);

        if (options.EnableConsole)
        {
            loggerConfiguration.WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] " +
                "[{ApplicationName}] " +
                "{Message:lj}" +
                "{NewLine}{Exception}");
        }

        if (options.EnableFile)
{
    // APP LOG
    loggerConfiguration.WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Verbose();

        lc.Filter.ByIncludingOnly(x =>
                x.Properties.ContainsKey("LogFolder")
                &&
                x.Properties["LogFolder"]
                    .ToString()
                    .Trim('"') == LogFolder.Log)

            .WriteTo.Map(
                keyPropertyName:
                "LogKey",

                defaultKey:
                "Log_ALL",

                configure:
                (
                    logKey,
                    wt
                ) =>
                {
                    wt.File(
                        path:
                        LogPathBuilder.Build(
                            options,
                            appInfo,
                            country:
                            logKey.Replace(
                                "Log_",
                                ""),

                            logFolder:
                            LogFolder.Log),

                        restrictedToMinimumLevel:
                        LogEventLevel.Information,

                        rollingInterval:
                        ParseRollingInterval(
                            options.File.RollingInterval),

                        retainedFileCountLimit:
                        options.File
                            .RetainedFileCountLimit,

                        shared:
                        options.File.Shared,

                        flushToDiskInterval:
                        TimeSpan.FromSeconds(1),

                        outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                        "[{Level:u3}] " +
                        "{Message:lj}" +
                        "{NewLine}{Exception}");
                });
    });

    // ERROR LOG
    loggerConfiguration.WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Verbose();

        lc.Filter.ByIncludingOnly(x =>
                x.Properties.ContainsKey("LogFolder")
                &&
                x.Properties["LogFolder"]
                    .ToString()
                    .Trim('"') == LogFolder.LogError)

            .WriteTo.Map(
                keyPropertyName:
                "LogKey",

                defaultKey:
                "LogError_ALL",

                configure:
                (
                    logKey,
                    wt
                ) =>
                {
                    wt.File(
                        path:
                        LogPathBuilder.Build(
                            options,
                            appInfo,
                            country:
                            logKey.Replace(
                                "LogError_",
                                ""),

                            logFolder:
                            LogFolder.LogError),

                        restrictedToMinimumLevel:
                        LogEventLevel.Information,

                        rollingInterval:
                        ParseRollingInterval(
                            options.File.RollingInterval),

                        retainedFileCountLimit:
                        options.File
                            .RetainedFileCountLimit,

                        shared:
                        options.File.Shared,

                        flushToDiskInterval:
                        TimeSpan.FromSeconds(1),

                        outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                        "[{Level:u3}] " +
                        "{Message:lj}" +
                        "{NewLine}{Exception}");
                });
    });

    // MIDDLEWARE LOG
    loggerConfiguration.WriteTo.Logger(lc =>
    {
        lc.MinimumLevel.Verbose();

        lc.Filter.ByIncludingOnly(x =>
                x.Properties.ContainsKey("LogFolder")
                &&
                x.Properties["LogFolder"]
                    .ToString()
                    .Trim('"') == LogFolder.LogMiddleware)

            .WriteTo.Map(
                keyPropertyName:
                "LogKey",

                defaultKey:
                "LogMiddleware_ALL",

                configure:
                (
                    logKey,
                    wt
                ) =>
                {
                    wt.File(
                        path:
                        LogPathBuilder.Build(
                            options,
                            appInfo,
                            country:
                            logKey.Replace(
                                "LogMiddleware_",
                                ""),

                            logFolder:
                            LogFolder.LogMiddleware),

                        restrictedToMinimumLevel:
                        LogEventLevel.Information,

                        rollingInterval:
                        ParseRollingInterval(
                            options.File.RollingInterval),

                        retainedFileCountLimit:
                        options.File
                            .RetainedFileCountLimit,

                        shared:
                        options.File.Shared,

                        flushToDiskInterval:
                        TimeSpan.FromSeconds(1),

                        outputTemplate:
                        "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} " +
                        "[{Level:u3}] " +
                        "{Message:lj}" +
                        "{NewLine}{Exception}");
                });
    });
}

        Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static RollingInterval ParseRollingInterval(string? value)
    {
        return value?.ToLower() switch
        {
            "hour" =>
                RollingInterval.Hour,

            "minute" =>
                RollingInterval.Minute,

            "month" =>
                RollingInterval.Month,

            "year" =>
                RollingInterval.Year,

            _ =>
                RollingInterval.Day
        };
    }
}