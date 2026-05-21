namespace Observix.Logging.Models;

public class LoggingOptions
{
    public const string SectionName = "Observix";

    public string EnvironmentName { get; set; } = default!;

    public string Provider { get; set; } = "Serilog";

    public string? BasePath { get; set; }

    public bool EnableConsole { get; set; } = true;

    public bool EnableFile { get; set; } = true;
    
    public int MaxBodyLength { get; set; } = 5000;
    
    public FileLoggingOptions File { get; set; } = new();
}