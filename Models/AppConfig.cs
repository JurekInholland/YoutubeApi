namespace Models;

/// <summary>
/// Configuration supplied via environment variables.
/// </summary>
public class AppConfig
{
    public string DataPath { get; set; } = string.Empty;
    public string YtDlPath { get; set; } = string.Empty;
    public string YoutubeApiKey { get; set; } = string.Empty;
}
