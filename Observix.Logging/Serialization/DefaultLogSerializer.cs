using System.Text.Encodings.Web;
using System.Text.Json;

namespace Observix.Logging.Serialization;

public class DefaultLogSerializer : ILogSerializer
{
    private static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, DefaultOptions);
    }
}