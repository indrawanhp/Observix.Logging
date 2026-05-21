using Observix.Logging.Helpers;
using Observix.Logging.Models;

namespace Observix.Logging.Context;

public class DefaultLogContextAccessor : ILogContextAccessor
{
    private readonly AppInfoOptions _appInfo;

    public DefaultLogContextAccessor(
        AppInfoOptions appInfo)
    {
        _appInfo = appInfo;
    }

    private static readonly AsyncLocal<LogContextModel?>
        ContextHolder = new();

    public virtual LogContextModel Get()
    {
        ContextHolder.Value ??=
            new LogContextModel();

        ContextHolder.Value.TraceId ??=
            Guid.NewGuid()
                .ToString();

        ContextHolder.Value.HostName ??= Environment.MachineName;

        ContextHolder.Value.InstanceId ??=
            Environment.GetEnvironmentVariable(
                "HOSTNAME")
            ?? Environment.MachineName;

        ContextHolder.Value.HostIpAddress ??= HostIpResolver.Resolve();

        ContextHolder.Value.EnvironmentName ??=
            Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT");

        ContextHolder.Value.Username ??= Environment.UserName;

        ContextHolder.Value.ApplicationName ??= _appInfo.FullName;

        return ContextHolder.Value;
    }

    public void SetCorrelationId(string correlationId)
    {
        var context = Get();

        context.CorrelationId = correlationId;

        ContextHolder.Value = context;
    }

    public void Set(LogContextModel context)
    {
        ContextHolder.Value = context;
    }
}