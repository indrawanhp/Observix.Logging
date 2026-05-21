namespace Observix.Logging.Context;

public sealed class LogContextModel
{
    public string TraceId { get; set; } = Guid.NewGuid().ToString();

    public string? CorrelationId { get; set; }
    
    public string? InstanceId { get; set; }

    public string? RemoteIpAddress { get; set; }

    public string? HostName { get; set; }
    
    public string? HostIpAddress { get; set; }

    public string? EnvironmentName { get; set; }

    public string? ApplicationName { get; set; }

    public string? Username { get; set; }

    public string? Country { get; set; }
}