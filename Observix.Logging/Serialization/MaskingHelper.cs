using System.Text.Json.Nodes;

namespace Observix.Logging.Serialization;

public static class MaskingHelper
{
    public static JsonNode? Mask(
        JsonNode? node,
        MaskingOptions options)
    {
        if (node is JsonObject obj)
        {
            foreach (var key in obj.ToList())
            {
                if (options.SensitiveKeys.Any(x =>
                        x.Equals(key.Key,
                            StringComparison.OrdinalIgnoreCase)))
                {
                    obj[key.Key] = options.MaskValue;
                    continue;
                }

                obj[key.Key] = Mask(key.Value, options);
            }
        }

        if (node is JsonArray arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                arr[i] =
                    Mask(arr[i], options)?
                        .DeepClone();
            }
        }

        return node;
    }
}