using Observix.Logging.Models;

namespace Observix.Logging.Logging;

public interface ILoggingService
{
    LogEntry Initial(
        string serviceLayer,
        string functionLayer,
        string className,
        string methodName,
        object? requestBody = null,
        object? requestHeader = null,
        object? queryParam = null,
        string? country = null,
        string? serviceName = null);

    LogEntry Finalize(
        LogEntry logEntry,
        bool isSuccess,
        string message,
        string eventType,
        string? responseCode = null,
        object? responseBody = null,
        object? queryParam = null,
        Exception? exception = null);

    void Info<TContext>(
        LogEntry logEntry);

    void Warning<TContext>(
        LogEntry logEntry);

    void Error<TContext>(
        LogEntry logEntry);

    string Serialize<T>(
        T value);

    string GetTraceId();
}