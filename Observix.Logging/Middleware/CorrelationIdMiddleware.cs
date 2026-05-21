using Microsoft.AspNetCore.Http;
using Observix.Logging.Context;
using Observix.Logging.Helpers;

namespace Observix.Logging.Middleware;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "x-correlation-id";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILogContextAccessor accessor)
    {
        var request = context.Request;
        
        var correlationId =
            context.Request.Headers[
                    HeaderName]
                .FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId =
                Guid.NewGuid()
                    .ToString();
        }

        var country = CountryHeaderResolver.Resolve(request);

        accessor.Set(
            new LogContextModel
            {
                CorrelationId = correlationId,

                Country = country,

                TraceId = Guid.NewGuid().ToString()
            });

        context.Response.Headers[HeaderName] = correlationId;

        await _next(context);
    }
}