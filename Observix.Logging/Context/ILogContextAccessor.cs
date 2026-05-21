namespace Observix.Logging.Context;

public interface ILogContextAccessor
{
    LogContextModel Get();
    void SetCorrelationId(string correlationId);
}