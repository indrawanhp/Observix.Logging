using Observix.Logging.Models;

namespace Observix.Logging.Context;

public class WorkerLogContextAccessor : DefaultLogContextAccessor
{
    public WorkerLogContextAccessor(AppInfoOptions appInfo) : base(appInfo)
    {
    }
}