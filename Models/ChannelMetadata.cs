namespace Models;

public class ChannelMetadata : BaseEntity
{
    public int VideoCount { get; set; }
    public string AvatarUrl { get; set; } = string.Empty;
    public string BannerUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
