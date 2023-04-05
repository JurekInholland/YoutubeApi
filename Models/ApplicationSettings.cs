namespace Models;

/// <summary>
/// Global application settings
/// </summary>
public class ApplicationSettings : BaseEntity
{
    public string NamingFormat { get; set; } = string.Empty;
    public string DownloadPath { get; set; } = string.Empty;
    public TimeSpan MaxVideoDuration { get; set; }
}
