using Observix.Logging.Models;

namespace Observix.Logging.Providers;

public interface ILoggingProvider
{
    void Info<TContext>(LogEntry logEntry);

    void Warning<TContext>(LogEntry logEntry);

    void Error<TContext>(LogEntry logEntry);
}