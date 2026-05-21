# Observix.Logging

Lightweight enterprise-grade observability library for .NET applications with structured logging, correlation ID propagation, request tracing, middleware logging, exception handling, sensitive data masking, country-based log routing, and Serilog integration.

---

# Features

## Core Features

- Structured JSON Logging
- Correlation ID Propagation
- Trace ID Support
- Request & Response Logging
- Exception Handling Middleware
- Middleware Pipeline Logging
- Enterprise Log Format
- Response Duration Tracking
- Environment Logging
- Source Context Logging

---

## Logging Features

- Console Logging
- File Logging
- Country-Based Log Routing
- Log Folder Separation
- Daily Rolling Logs
- Configurable Retention
- Configurable Output Template
- Structured JSON Output
- Compact Enterprise Logging

---

## Security Features

- Sensitive Data Masking
- Password Masking
- Token Masking
- Secret Masking
- API Key Masking

---

# Supported Application Types

| Application Type | Supported |
|---|---|
| ASP.NET Core API | ✅ |
| Minimal API | ✅ |
| Worker Service | ✅ |
| Console Application | ✅ |
| Background Service | ✅ |

---

# Installation

```bash
dotnet add package Observix.Logging
```

---

# Dependencies

- .NET 10
- Serilog
- Serilog.AspNetCore
- Serilog.Sinks.Console
- Serilog.Sinks.File

---

# Quick Start

## 1. Add Configuration

Add this to `appsettings.json`

```json
{
  "AppInfo": {
    "FullName": "my-service"
  },

  "Observix": {
    "Provider": "Serilog",

    "EnableConsole": true,

    "EnableFile": true,

    "EnvironmentName": "Development",

    "MaxBodyLength": 5000,

    "File": {
      "BasePath": "C:\\deployment",

      "Template": "{AppName}/logs/app/{LogFolder}/{Year-Month}/{Day}/Log_{Country}.log",

      "RollingInterval": "Day",

      "RetainedFileCountLimit": 30,

      "Shared": true
    }
  }
}
```

---

# ASP.NET Core API Usage

## Program.cs

```csharp
using Observix.Logging.Extensions;

var builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseObservix();

builder.Services.AddObservix(
    builder.Configuration);

var app = builder.Build();

app.UseObservix();

app.MapControllers();

app.Run();
```

---

# Minimal API Usage

## Program.cs

```csharp
using Observix.Logging.Extensions;
using Observix.Logging.Logging;
using Observix.Logging.Models;

var builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseObservix();

builder.Services.AddObservix(
    builder.Configuration);

var app = builder.Build();

app.UseObservix();

app.MapGet(
    "/business",
    (
        ILoggingService logging
    ) =>
    {
        var log =
            logging.Initial(
                serviceLayer:
                ServiceLayer.Api,

                functionLayer:
                FunctionLayer.Business,

                className:
                "BusinessEndpoint",

                methodName:
                "GetBusiness",

                requestBody:
                new
                {
                    username = "Maverick",
                    password = "123456"
                });

        logging.Info<Program>(
            logging.Finalize(
                log,
                true,
                "Business Success",
                EventType.Information,
                responseBody:
                new
                {
                    result = "OK"
                }));

        return Results.Ok(
            new
            {
                message = "Business Success"
            });
    });

app.Run();
```

---

# Worker Service Usage

## Program.cs

```csharp
using Observix.Logging.Extensions;

var builder =
    Host.CreateApplicationBuilder(args);

builder.Host.UseObservix();

builder.Services.AddObservix(
    builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
```

---

## Worker.cs

```csharp
using Observix.Logging.Logging;
using Observix.Logging.Models;

public class Worker : BackgroundService
{
    private readonly ILoggingService _logging;

    public Worker(
        ILoggingService logging)
    {
        _logging = logging;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var log =
                _logging.Initial(
                    serviceLayer:
                    ServiceLayer.Worker,

                    functionLayer:
                    FunctionLayer.Job,

                    className:
                    nameof(Worker),

                    methodName:
                    nameof(ExecuteAsync));

            _logging.Info<Worker>(
                _logging.Finalize(
                    log,
                    true,
                    "Worker Running",
                    EventType.Information));

            await Task.Delay(
                5000,
                stoppingToken);
        }
    }
}
```

---

# Console Application Usage

## Program.cs

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Observix.Logging.Extensions;
using Observix.Logging.Logging;
using Observix.Logging.Models;

var builder =
    Host.CreateApplicationBuilder(args);

builder.Host.UseObservix();

builder.Services.AddObservix(
    builder.Configuration);

var host =
    builder.Build();

var logging =
    host.Services
        .GetRequiredService<
            ILoggingService>();

var log =
    logging.Initial(
        serviceLayer:
        ServiceLayer.Console,

        functionLayer:
        FunctionLayer.Business,

        className:
        "Program",

        methodName:
        "Main");

logging.Info<Program>(
    logging.Finalize(
        log,
        true,
        "Console Started",
        EventType.Information));

Console.ReadLine();
```

---

# Built-in Middleware

| Middleware | Description |
|---|---|
| CorrelationIdMiddleware | Correlation ID propagation |
| RequestLoggingMiddleware | Request & response logging |
| ExceptionHandlingMiddleware | Global exception handling |

---

# Correlation ID Propagation

Observix automatically supports correlation ID propagation across services.

## Supported Request Headers

```http
x-correlation-id
```

## Behavior

| Scenario | Behavior |
|---|---|
| Header exists | Reuse incoming correlation ID |
| Header missing | Generate new correlation ID |
| Response sent | Correlation ID returned in response header |

---

## Example Request

```http
GET /business HTTP/1.1
Host: localhost:5277
x-correlation-id: abc-123
```

## Example Response

```http
HTTP/1.1 200 OK
x-correlation-id: abc-123
```

---

## Example Generated Correlation ID

```http
x-correlation-id: 8d4f7c5e-3f4e-4c8f-a91d-7f3d82d1e6ab
```

---

# Country-Based Log Routing

Supports automatic log routing using:

- x-country
- x-obras-country
- x-dnet-country

Example request:

```http
x-country: ID
```

Generated log file:

```txt
Log_ID.log
```

---

# Sensitive Data Masking

Automatically masks sensitive fields.

## Supported Fields

- password
- pin
- token
- secret
- apiKey

Example:

```json
{
  "password": "******"
}
```

---

# Example Log Output

```log
2026-05-21 09:26:17.134 +07:00 [INF]
{
  "traceId":"c94d1ce6-c745-46b1-8078-e43fe9988cc0",
  "correlationId":"d77ee78e-830d-4772-990b-0fe9efe7fb97",
  "instanceId":"DESKTOP-AQDR2FM",
  "serviceLayer":"Api",
  "functionLayer":"Controller",
  "serviceName":"test-api-minimal",
  "className":"HTTP",
  "methodName":"GET /business",
  "country":"ID",
  "responseMillis":203,
  "isSuccess":true,
  "logMessage":"Request Success",
  "eventType":"Information"
}
```

---

# Generated Log Structure

```txt
C:\deployment
└── my-service
    └── logs
        └── app
            ├── Log
            ├── LogError
            └── LogMiddleware
```

---

# Generated File Example

```txt
Log_ID.log
Log_TW.log
Log_ALL.log
```

---

# Configuration Reference

| Property | Description |
|---|---|
| EnableConsole | Enable console logging |
| EnableFile | Enable file logging |
| EnvironmentName | Current environment |
| MaxBodyLength | Max request/response body length |
| BasePath | Root log directory |
| Template | Log file template |
| RollingInterval | Rolling interval |
| RetainedFileCountLimit | Max retained log files |
| Shared | Shared file access |

---

# Log Folder Types

| Folder | Description |
|---|---|
| Log | Business logs |
| LogError | Error logs |
| LogMiddleware | Middleware logs |

---

# Recommended Usage

Recommended for:

- Banking Services
- Enterprise APIs
- Internal Platforms
- Microservices
- Distributed Systems
- Worker Services
- Middleware Libraries

---

# Package Information

| Property | Value |
|---|---|
| Package | Observix.Logging |
| Version | 1.0.0 |
| Framework | .NET 10 |
| License | MIT |

---

# Future Roadmap

- OpenTelemetry Integration
- ElasticSearch Sink
- Loki Integration
- Seq Integration
- Grafana Dashboard
- Distributed Tracing
- Metrics Support
- Prometheus Exporter

---

# Author

Indrawan Heri Prabowo

---

Made with ☕ + production incident trauma + enterprise debugging.