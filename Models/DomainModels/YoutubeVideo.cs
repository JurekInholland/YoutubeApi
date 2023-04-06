using System.Text.Json.Serialization;

namespace Models.DomainModels;

public class YoutubeVideo : BaseEntity
{
    public string Title { get; set; } = "";
    public string Thumbnail { get; set; } = "";

    public string YoutubeThumbnailUrl => $"https://i.ytimg.com/vi/{Id}/maxresdefault.jpg";

    public string Description { get; set; } = "";
    // public string Uploader { get; set; } = "";

    public DateTime DateAdded { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime UploadDate { get; set; }

    // [JsonPropertyName("uploader_id")] public string UploaderId { get; set; } = "";
    public TimeSpan Duration { get; set; }

    // public string DurationString { get; set; } = "";
    // public string ChannelId { get; set; } = "";
    // public string ChannelUrl { get; set; } = "";
    public string WebpageUrl { get; set; } = "";
    public long ViewCount { get; set; }
    public long LikeCount { get; set; }

    public string[] Categories { get; set; } = {""};
    public string[] RelatedVideos { get; set; } = {""};

    // public int Width { get; set; }
    // public int Height { get; set; }
    // public int Fps { get; set; }
    //
    // [JsonPropertyName("filesize_approx")] public int FileSize { get; set; }
    // public float Vbr { get; set; }
    // public float Abr { get; set; }

    public List<YoutubeComment> Comments { get; set; } = new();

    public virtual LocalVideo? LocalVideo { get; set; }

    public virtual YoutubeChannel YoutubeChannel { get; set; }
}
