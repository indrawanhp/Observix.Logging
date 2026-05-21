using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Observix.Logging.Context;
using Observix.Logging.Logging;
using Observix.Logging.Models;

namespace Observix.Logging.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILoggingService logging,
        ILogContextAccessor accessor)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var logContext =
                accessor.Get();

            var response =
                new ErrorResponse
                {
                    TraceId =
                        logContext.TraceId,

                    CorrelationId =
                        logContext.CorrelationId,

                    ResponseCode =
                        StatusCodes
                            .Status500InternalServerError
                            .ToString(),

                    Message =
                        "Internal Server Error"
                };

            context.Response.ContentType =
                "application/json";

            context.Response.StatusCode =
                StatusCodes
                    .Status500InternalServerError;

            var log =
                logging.Initial(
                    serviceLayer:
                    ServiceLayer.Api,

                    functionLayer:
                    FunctionLayer.Controller,

                    className:
                    nameof(ExceptionHandlingMiddleware),

                    methodName:
                    context.Request.Path
                );

            log.LogFolder =
                LogFolder.LogError;
            
            logging.Error<
                ExceptionHandlingMiddleware>(
                logging.Finalize(
                    log,
                    false,
                    ex.Message,
                    EventType.Error,
                    responseCode:
                    StatusCodes
                        .Status500InternalServerError
                        .ToString(),

                    exception:
                    ex
                ));
            
            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }

    private sealed class ErrorResponse
    {
        public string? TraceId { get; set; }

        public string? CorrelationId { get; set; }

        public string? ResponseCode { get; set; }

        public string? Message { get; set; }
    }
}