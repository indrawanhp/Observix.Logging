using Serilog.Core;
using Serilog.Events;

namespace Observix.Logging.Providers.Serilog.Enrichers;

public class CorrelationIdEnricher : ILogEventEnricher
{
    private readonly Func<string?> _correlationAccessor;

    public CorrelationIdEnricher(
        Func<string?> correlationAccessor)
    {
        _correlationAccessor = correlationAccessor;
    }

    public void Enrich(
        LogEvent logEvent,
        ILogEventPropertyFactory propertyFactory)
    {
        var correlationId =
            _correlationAccessor();

        if (string.IsNullOrWhiteSpace(correlationId))
            return;

        var property = propertyFactory.CreateProperty(
            "CorrelationId",
            correlationId);

        logEvent.AddPropertyIfAbsent(property);
    }
}