namespace Observix.Logging.Serialization;

public class MaskingOptions
{
    public List<string> SensitiveKeys { get; set; } =
    [
        "password",
        "pin",
        "token",
        "secret",
        "authorization",
        "accessToken",
        "refreshToken",
        "accountNumber",
        "cardNumber"
    ];

    public string MaskValue { get; set; } = "******";
}