using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Observix.Logging.Providers.Serilog.Enrichers;

public class TraceIdEnricher : ILogEventEnricher
{
    public void Enrich(
        LogEvent logEvent,
        ILogEventPropertyFactory propertyFactory)
    {
        var activity = Activity.Current;

        if (activity is null)
            return;

        var traceIdProperty =
            propertyFactory.CreateProperty(
                "TraceId",
                activity.TraceId.ToString());

        var spanIdProperty =
            propertyFactory.CreateProperty(
                "SpanId",
                activity.SpanId.ToString());

        logEvent.AddPropertyIfAbsent(
            traceIdProperty);

        logEvent.AddPropertyIfAbsent(
            spanIdProperty);
    }
}