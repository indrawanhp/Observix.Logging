using Microsoft.AspNetCore.Http;
using Observix.Logging.Context;

namespace Observix.Logging.Middleware;

public class CorrelationIdMiddleware
{
    private const string HeaderName =
        "x-correlation-id";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogContextAccessor accessor)
    {
        var correlationId =
            context.Request.Headers[HeaderName]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(
                correlationId))
        {
            correlationId =
                Guid.NewGuid().ToString();
        }

        accessor.SetCorrelationId(
            correlationId);

        context.Response.Headers[HeaderName] =
            correlationId;

        await _next(context);
    }
}