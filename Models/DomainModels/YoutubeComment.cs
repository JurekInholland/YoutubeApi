using System.Text.Json.Serialization;

namespace Models.DomainModels;

public class YoutubeComment : BaseEntity
{
    public string Text { get; set; } = "";
    public string Author { get; set; } = "";

    [JsonPropertyName("author_id")]
    public string AuthorId { get; set; } = "";

    [JsonPropertyName("author_thumbnail")]
    public string AuthorThumbnail { get; set; } = "";
    public bool AuthorIsUploader { get; set; }


    [JsonPropertyName("like_count")]
    public int LikeCount { get; set; }
    // public string TimeStamp { get; set; }

    [JsonPropertyName("time_text")]
    public string TimeText { get; set; } = "";

    [JsonPropertyName("parent_id")]
    public string ParentId { get; set; } = "";

    public bool IsFavorited { get; set; }
}
