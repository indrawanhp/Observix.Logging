using Observix.Logging.Models;
using Observix.Logging.Providers.Serilog.Helpers;
using Observix.Logging.Serialization;
using Serilog;

namespace Observix.Logging.Providers.Serilog;

public class SerilogLoggingProvider : ILoggingProvider
{
    private readonly ILogSerializer _serializer;

    public SerilogLoggingProvider(
        ILogSerializer serializer)
    {
        _serializer = serializer;
    }

    public void Info<TContext>(
        LogEntry logEntry)
    {
        using var _ =
            new LogContextScope(
                logEntry);

        Log.ForContext<TContext>()
            .Information(
                "{Message:lj}",
                _serializer.Serialize(
                    logEntry));
    }

    public void Warning<TContext>(
        LogEntry logEntry)
    {
        using var _ =
            new LogContextScope(
                logEntry);

        Log.ForContext<TContext>()
            .Warning(
                "{Message:lj}",
                _serializer.Serialize(
                    logEntry));
    }

    public void Error<TContext>(
        LogEntry logEntry)
    {
        using var _ =
            new LogContextScope(
                logEntry);

        Log.ForContext<TContext>()
            .Error(
                "{Message:lj}",
                _serializer.Serialize(
                    logEntry));
    }
}