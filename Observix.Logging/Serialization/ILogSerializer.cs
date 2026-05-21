namespace Observix.Logging.Serialization;

public interface ILogSerializer
{
    string Serialize<T>(T value);
}