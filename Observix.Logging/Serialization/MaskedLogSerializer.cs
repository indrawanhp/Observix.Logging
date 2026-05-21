using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Observix.Logging.Serialization;

public class MaskedLogSerializer : ILogSerializer
{
    private readonly MaskingOptions _maskingOptions;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public MaskedLogSerializer(MaskingOptions maskingOptions)
    {
        _maskingOptions = maskingOptions;
    }

    public string Serialize<T>(T value)
    {
        var rawJson = JsonSerializer.Serialize(value, JsonOptions);

        var node = JsonNode.Parse(rawJson);

        var masked = MaskingHelper.Mask(node, _maskingOptions);

        return masked?.ToJsonString(JsonOptions)
               ?? "{}";
    }
}