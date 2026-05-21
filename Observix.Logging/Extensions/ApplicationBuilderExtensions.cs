using Microsoft.AspNetCore.Builder;
using Observix.Logging.Middleware;

namespace Observix.Logging.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseObservix(this IApplicationBuilder app)
    {
        app.UseMiddleware<
            CorrelationIdMiddleware>();
        
        app.UseMiddleware<
            ExceptionHandlingMiddleware>();
        
        app.UseMiddleware<
            RequestLoggingMiddleware>();

        return app;
    }
}