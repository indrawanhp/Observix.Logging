using Observix.Logging.Context;
using Observix.Logging.Models;
using Observix.Logging.Providers;
using Observix.Logging.Serialization;

namespace Observix.Logging.Logging;

public class LoggingService : ILoggingService
{
    private readonly ILogContextAccessor _contextAccessor;

    private readonly ILogSerializer _serializer;

    private readonly ILoggingProvider _provider;

    public LoggingService(
        ILogContextAccessor contextAccessor,
        ILogSerializer serializer,
        ILoggingProvider provider)
    {
        _contextAccessor = contextAccessor;

        _serializer = serializer;

        _provider = provider;
    }

    public LogEntry Initial(
        string serviceLayer,
        string functionLayer,
        string className,
        string methodName,
        object? requestBody = null,
        object? requestHeader = null,
        object? queryParam = null,
        string? country = null,
        string? serviceName = null)
    {
        var context = _contextAccessor.Get();

        return new LogEntry
        {
            TraceId = context.TraceId,

            CorrelationId = context.CorrelationId,
            
            InstanceId = context.InstanceId,

            RemoteIpAddress = context.RemoteIpAddress,

            HostName = context.HostName,

            Username = context.Username,

            Country =
                country
                ?? context.Country
                ?? LoggingConstants.DefaultCountry,

            ServiceLayer = serviceLayer,

            FunctionLayer = functionLayer,

            ServiceName =
                serviceName
                ?? context.ApplicationName
                ?? LoggingConstants.Unknown,

            Environment = context.EnvironmentName,
            
            ClassName = className,

            MethodName = methodName,

            RequestTime = DateTimeOffset.UtcNow,

            RequestHeader = requestHeader,

            RequestBody = requestBody,

            QueryParam = queryParam,
            
            SourceContext =
                className,
        };
    }

    public LogEntry Finalize(
        LogEntry logEntry,
        bool isSuccess,
        string message,
        string eventType,
        string? responseCode = null,
        object? responseBody = null,
        object? queryParam = null,
        Exception? exception = null)
    {
        logEntry.IsSuccess = isSuccess;

        logEntry.LogMessage = message;

        logEntry.EventType = eventType;

        logEntry.ResponseCode = responseCode;

        logEntry.ResponseBody = responseBody;

        logEntry.QueryParam =
            queryParam
            ?? logEntry.QueryParam;

        logEntry.ResponseTime =
            DateTimeOffset.UtcNow;

        logEntry.ResponseMillis =
            (long)(logEntry.ResponseTime.Value
                   - logEntry.RequestTime)
            .TotalMilliseconds;
        
        logEntry.ResponseDuration =
            $"{logEntry.ResponseMillis}ms";

        if (exception is not null)
        {
            logEntry.ExceptionMessage =
                exception.Message;

            logEntry.StackTrace =
                exception.ToString();
        }

        return logEntry;
    }

    public void Info<TContext>(
        LogEntry logEntry)
    {
        logEntry.LogFolder ??=
            LogFolder.Log;
        
        _provider.Info<TContext>(logEntry);
    }

    public void Warning<TContext>(
        LogEntry logEntry)
    {
        logEntry.LogFolder ??=
            LogFolder.Log;
        
        _provider.Warning<TContext>(logEntry);
    }

    public void Error<TContext>(
        LogEntry logEntry)
    {
        logEntry.LogFolder ??=
            LogFolder.LogError;
        
        _provider.Error<TContext>(logEntry);
    }

    public string Serialize<T>(
        T value)
    {
        return _serializer.Serialize(value);
    }

    public string GetTraceId()
    {
        return _contextAccessor
            .Get()
            .TraceId;
    }
}