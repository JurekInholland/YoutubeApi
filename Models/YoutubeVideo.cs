using System.Text.Json.Serialization;

namespace Models;

public class YoutubeVideo : BaseEntity
{
    public string Title { get; set; } = "";
    public string Thumbnail { get; set; } = "";
    public string Description { get; set; } = "";
    public string Uploader { get; set; } = "";

    [JsonPropertyName("uploader_id")]
    public string UploaderId { get; set; } = "";
    public int Duration { get; set; }
    public string DurationString { get; set; } = "";
    public string ChannelId { get; set; } = "";
    public string ChannelUrl { get; set; } = "";
    public string WebpageUrl { get; set; } = "";
    public int ViewCount { get; set; }
    public List<string> Categories { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Fps { get; set; }

    [JsonPropertyName("filesize_approx")]
    public int FileSize { get; set; }
    public float Vbr { get; set; }
    public float Abr { get; set; }
}
