using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.DomainModels;

public class QueuedDownload : BaseEntity
{
    public Enums.DownloadStatus Status { get; set; }
    public DateTime QueuedAt { get; set; }


    public virtual YoutubeVideo Video { get; set; } = null!;
}
