namespace Models;

/// <summary>
/// Global application settings
/// </summary>
public class ApplicationSettings : BaseEntity
{
    public string NamingFormat { get; set; } = string.Empty;
    public string DownloadPath { get; set; } = string.Empty;
    public TimeSpan MaxVideoDuration { get; set; }
    public long CleanUpInterval { get; set; }

    public long UpdateChannelsIntervalSeconds { get; set; }
    public long UpdateVideosIntervalSeconds { get; set; }
    public long CheckForNewVideosIntervalSeconds { get; set; }
    public long BackupIntervalSeconds { get; set; }


    public long WorkInterval { get; set; }
}
