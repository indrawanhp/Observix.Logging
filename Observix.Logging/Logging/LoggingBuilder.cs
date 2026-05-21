using Observix.Logging.Models;

namespace Observix.Logging.Logging;

public class LoggingBuilder
{
    public LoggingOptions Options { get; } = new();

    public LoggingBuilder UseProvider(string provider)
    {
        Options.Provider = provider;
        return this;
    }

    public LoggingBuilder EnableConsole(
        bool enabled = true)
    {
        Options.EnableConsole = enabled;
        return this;
    }

    public LoggingBuilder EnableFile(
        bool enabled = true)
    {
        Options.EnableFile = enabled;
        return this;
    }
}