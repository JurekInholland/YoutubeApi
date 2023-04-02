namespace Models;

public class QueuedDownload : BaseEntity
{
    public Enums.DownloadStatus Status { get; set; }
    public DateTime QueuedAt { get; set; }
    public YoutubeVideo Video { get; set; }
}
