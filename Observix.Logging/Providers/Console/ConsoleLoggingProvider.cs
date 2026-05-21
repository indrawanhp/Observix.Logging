using Observix.Logging.Models;
using Observix.Logging.Serialization;

namespace Observix.Logging.Providers.Console;

public class ConsoleLoggingProvider : ILoggingProvider
{
    private readonly ILogSerializer _serializer;

    public ConsoleLoggingProvider(
        ILogSerializer serializer)
    {
        _serializer = serializer;
    }

    public void Info<TContext>(LogEntry logEntry)
    {
        System.Console.WriteLine(
            $"[INFO] [{typeof(TContext).Name}] {_serializer.Serialize(logEntry)}");
    }

    public void Warning<TContext>(LogEntry logEntry)
    {
        System.Console.WriteLine(
            $"[WARN] [{typeof(TContext).Name}] {_serializer.Serialize(logEntry)}");
    }

    public void Error<TContext>(LogEntry logEntry)
    {
        System.Console.WriteLine(
            $"[ERROR] [{typeof(TContext).Name}] {_serializer.Serialize(logEntry)}");
    }
}