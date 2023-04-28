namespace Models.DomainModels;

public class YoutubePlaylist : BaseEntity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int VideoCount { get; set; }
    public string ViewCount { get; set; }
    public string LastUpdated { get; set; }
    public ICollection<YoutubeVideo> Videos { get; set; }
}
