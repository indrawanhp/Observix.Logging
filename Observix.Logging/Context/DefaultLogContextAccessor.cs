using Observix.Logging.Helpers;
using Observix.Logging.Models;

namespace Observix.Logging.Context;

public class DefaultLogContextAccessor : ILogContextAccessor
{
    private readonly AppInfoOptions _appInfo;

    public DefaultLogContextAccessor(AppInfoOptions appInfo)
    {
        _appInfo = appInfo;
    }

    private static readonly AsyncLocal<string?>
        CorrelationIdHolder = new();

    private static readonly AsyncLocal<string?>
        TraceIdHolder = new();

    public virtual LogContextModel Get()
    {
        TraceIdHolder.Value ??=
            Guid.NewGuid().ToString();

        return new LogContextModel
        {
            TraceId = TraceIdHolder.Value,

            HostName = Environment.MachineName,
            
            InstanceId =
                Environment.GetEnvironmentVariable("HOSTNAME")
                ?? Environment.MachineName,
            
            HostIpAddress = HostIpResolver.Resolve(),

            EnvironmentName =
                Environment.GetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT"),

            Username = Environment.UserName,

            ApplicationName =
                _appInfo.FullName,

            CorrelationId =
                CorrelationIdHolder.Value
        };
    }

    public void SetCorrelationId(
        string correlationId)
    {
        CorrelationIdHolder.Value =
            correlationId;
    }
}