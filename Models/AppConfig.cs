namespace Models;

/// <summary>
/// Configuration supplied via environment variables.
/// </summary>
public class AppConfig
{
    public string DataPath { get; set; }
    public string YtDlPath { get; set; }
    public string YoutubeApiKey { get; set; }
}
