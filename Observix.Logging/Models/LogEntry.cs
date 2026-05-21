using System.Text.Json.Serialization;

namespace Observix.Logging.Models;

public class LogEntry
{
    public string TraceId { get; set; } = default!;

    public string? CorrelationId { get; set; }

    public string? InstanceId { get; set; }

    public string? RemoteIpAddress { get; set; }

    public string? HostName { get; set; }

    public string? Username { get; set; }

    public string? Country { get; set; }

    public string ServiceLayer { get; set; } = default!;

    public string FunctionLayer { get; set; } = default!;

    public string ServiceName { get; set; } = default!;

    public string ClassName { get; set; } = default!;

    public string MethodName { get; set; } = default!;

    public DateTimeOffset RequestTime { get; set; }

    public DateTimeOffset? ResponseTime { get; set; }

    public long? ResponseMillis { get; set; }

    public object? RequestHeader { get; set; }

    public object? RequestBody { get; set; }

    public object? ResponseBody { get; set; }

    public object? QueryParam { get; set; }

    public bool IsSuccess { get; set; }

    public string? LogMessage { get; set; }

    public string? EventType { get; set; }

    public string? ResponseCode { get; set; }

    public string? ExceptionMessage { get; set; }

    public string? StackTrace { get; set; }
    
    public string? LogFolder { get; set; }
    
    public string? Environment { get; set; }
    
    public string? ResponseDuration { get; set; }
    
    public string? SourceContext { get; set; }
}