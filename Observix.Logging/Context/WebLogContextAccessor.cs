using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Observix.Logging.Models;

namespace Observix.Logging.Context;

public class WebLogContextAccessor : DefaultLogContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebLogContextAccessor(
        IHttpContextAccessor httpContextAccessor,
        AppInfoOptions appInfo)
        : base(appInfo)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override LogContextModel Get()
    {
        var context = _httpContextAccessor.HttpContext;

        var traceId =
            context?.TraceIdentifier
            ?? Activity.Current?.TraceId.ToString()
            ?? Guid.NewGuid().ToString();

        return new LogContextModel
        {
            TraceId = traceId,
            CorrelationId = context?.Request.Headers["x-correlation-id"],
            RemoteIpAddress = context?.Connection.RemoteIpAddress?.ToString(),
            HostName = Environment.MachineName,
            EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            Username = context?.User?.Identity?.Name
        };
    }
}