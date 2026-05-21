using System.Text;
using Microsoft.AspNetCore.Http;
using Observix.Logging.Helpers;
using Observix.Logging.Logging;
using Observix.Logging.Models;

namespace Observix.Logging.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    private readonly LoggingOptions _options;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        LoggingOptions options)
    {
        _next = next;

        _options = options;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ILoggingService logging)
    {
        var request =
            context.Request;

        string? requestBody =
            await ReadRequestBodyAsync(
                request);

        var originalBodyStream =
            context.Response.Body;

        var responseBodyStream =
            new MemoryStream();

        context.Response.Body =
            responseBodyStream;
        
        var country = CountryHeaderResolver.Resolve(request);

        var log =
            logging.Initial(
                serviceLayer: ServiceLayer.Api,
                functionLayer: FunctionLayer.Controller,
                className: "HTTP",
                methodName: $"{request.Method} {request.Path}",
                country: country.ToUpper(),
                requestHeader: request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()), 
                requestBody: requestBody,
                queryParam: request.Query.ToDictionary(x => x.Key, x => x.Value.ToString())
            );
        
        log.LogFolder ??=
            LogFolder.LogMiddleware;

        try
        {
            await _next(context);

            var responseBody =
                await ReadResponseBodyAsync(
                    responseBodyStream);

            logging.Info<
                RequestLoggingMiddleware>(
                logging.Finalize(
                    log,
                    true,
                    "Request Success",
                    EventType.Information,
                    responseCode:
                    context.Response.StatusCode
                        .ToString(),

                    responseBody:
                    TryParseJson(responseBody)
                ));
        }
        finally
        {
            responseBodyStream.Position = 0;

            await responseBodyStream
                .CopyToAsync(
                    originalBodyStream);

            context.Response.Body =
                originalBodyStream;
            
            responseBodyStream.Dispose();
        }
    }

    private async Task<string?>
        ReadRequestBodyAsync(
            HttpRequest request)
    {
        if (request.ContentLength is null
            || request.ContentLength == 0
            || !request.Body.CanRead)
        {
            return null;
        }

        request.EnableBuffering();

        using var reader =
            new StreamReader(
                request.Body,
                Encoding.UTF8,
                leaveOpen: true);

        var body =
            await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return Truncate(body);
    }

    private async Task<string?> ReadResponseBodyAsync(MemoryStream stream)
    {
        stream.Position = 0;

        using var reader =
            new StreamReader(
                stream,
                leaveOpen: true);

        var body =
            await reader.ReadToEndAsync();

        stream.Position = 0;

        return Truncate(body);
    }

    private string Truncate(
        string value)
    {
        if (string.IsNullOrWhiteSpace(
                value))
        {
            return value;
        }

        if (value.Length
            <= _options.MaxBodyLength)
        {
            return value;
        }

        return
            value[.._options.MaxBodyLength]
            + "...[TRUNCATED]";
    }
    
    private static object? TryParseJson(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        try
        {
            return System.Text.Json.JsonSerializer
                .Deserialize<object>(value);
        }
        catch
        {
            return value;
        }
    }
}