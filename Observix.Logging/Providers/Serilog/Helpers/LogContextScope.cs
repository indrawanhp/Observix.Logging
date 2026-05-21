using Observix.Logging.Models;
using Serilog.Context;

namespace Observix.Logging.Providers.Serilog.Helpers;

public sealed class LogContextScope : IDisposable
{
    private readonly IDisposable _traceId;

    private readonly IDisposable _applicationName;

    private readonly IDisposable _logFolder;
    
    private readonly IDisposable _logKey;

    public LogContextScope(LogEntry logEntry)
    {
        _traceId =
            LogContext.PushProperty(
                "TraceId",
                logEntry.TraceId);

        _applicationName =
            LogContext.PushProperty(
                "ApplicationName",
                logEntry.ServiceName);

        _logFolder =
            LogContext.PushProperty(
                "LogFolder",
                logEntry.LogFolder);
        
        _logKey =
            LogContext.PushProperty(
                "LogKey",
                $"{logEntry.LogFolder}_{logEntry.Country}");
    }

    public void Dispose()
    {
        _logKey.Dispose();
        
        _logFolder.Dispose();

        _applicationName.Dispose();

        _traceId.Dispose();
    }
}