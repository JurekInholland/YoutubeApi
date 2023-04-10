namespace Models.Requests;

public class UpdateSettingsRequest
{
    public string? NamingFormat { get; set; }
    public string? DownloadPath { get; set; }
    public long? MaxVideoDuration { get; set; }
    public long? CleanUpInterval { get; set; }
    public long? WorkInterval { get; set; }
}
