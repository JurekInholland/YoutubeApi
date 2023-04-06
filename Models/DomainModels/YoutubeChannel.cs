namespace Models.DomainModels;

public class YoutubeChannel : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Handle { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
    public string ChannelUrl => $"https://www.youtube.com/channel/{Id}";
    public string Subscribers { get; set; } = string.Empty;

    public ICollection<YoutubeVideo>? Videos { get; set; }
}
