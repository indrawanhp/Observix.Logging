using System.Diagnostics;
using Observix.Logging.Models;

namespace Observix.Logging.Context;

public class ConsumerLogContextAccessor : DefaultLogContextAccessor
{
    private readonly string? _traceId;

    public ConsumerLogContextAccessor(AppInfoOptions appInfo, string? traceId = null) : base(appInfo)
    {
        _traceId = traceId;
    }

    public override LogContextModel Get()
    {
        var context = base.Get();

        context.TraceId =
            _traceId
            ?? Activity.Current?.TraceId.ToString()
            ?? Guid.NewGuid().ToString();

        return context;
    }
}