namespace Observix.Logging.Models;

public class FileLoggingOptions
{
    public string BasePath { get; set; } =
        string.Empty;

    public string Template { get; set; } =
        string.Empty;
    
    public string RollingInterval { get; set; } =
        "Day";

    public int RetainedFileCountLimit { get; set; } =
        30;

    public bool Shared { get; set; } =
        true;
}