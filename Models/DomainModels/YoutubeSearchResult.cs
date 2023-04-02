namespace Models.DomainModels;

public class YoutubeSearchResult : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ChannelTitle { get; set; } = string.Empty;
    public string ChannelId { get; set; } = string.Empty;
    // public YoutubeChannel Channel { get; set; }
    public TimeSpan Duration { get; set; }
    public string ThumbnailUrl { get; set; } = string.Empty;
}
