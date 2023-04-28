namespace Models.DomainModels;

public class YoutubeChannel : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string BannerUrl { get; set; } = string.Empty;
    public int SubscriberCount { get; set; }
    public int VideoCount { get; set; }
    public string ChannelUrl => $"https://www.youtube.com/channel/{Id}";
    public string Avatar => $"/api/Thumbnail/channel/{Id}";
    public string Banner => $"/api/Thumbnail/channel/{Id}";
    public string Description { get; set; } = string.Empty;

    public ICollection<YoutubeVideo>? Videos { get; set; }
}
