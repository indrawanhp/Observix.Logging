using Observix.Logging.Models;

namespace Observix.Logging.Providers.Serilog.Helpers;

public static class LogPathBuilder
{
    public static string Build(LoggingOptions options, AppInfoOptions appInfo, string? country = null, string? logFolder = null)
    {
        var now =
            DateTime.Now;

        var template =
            options.File.Template;

        var path =
            template
                .Replace(
                    "{AppName}",
                    appInfo.FullName)

                .Replace(
                    "{LogFolder}",
                    logFolder
                    ?? LogFolder.Log)

                .Replace(
                    "{Year-Month}",
                    now.ToString("yyyy-MM"))

                .Replace(
                    "{Day}",
                    now.ToString("dd"))

                .Replace(
                    "{Country}",
                    country ?? "ALL");

        var fullPath =
            Path.Combine(
                options.File.BasePath,
                path);

        var directory =
            Path.GetDirectoryName(
                fullPath);

        if (!string.IsNullOrWhiteSpace(
                directory))
        {
            Directory.CreateDirectory(
                directory);
        }

        return fullPath;
    }
}