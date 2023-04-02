namespace Models.DomainModels;

public class RelatedVideo : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string ChannelId { get; set; } = string.Empty;
    public string ChannelTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
    public DateTime PublishTime { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public string[] Tags { get; set; } = {""};


    // public RelatedVideo(VideoSearchResult searchResult)
    // {
    //     Id = searchResult.Id;
    //     Title = searchResult.Title;
    //     ChannelId = searchResult.Author.ChannelId;
    //     ChannelTitle = searchResult.Author.Title;
    //     // Description = searchResult.;
    //     Thumbnail = searchResult.Thumbnails.OrderByDescending(x => x.Resolution).First().Url;
    //     PublishTime = searchResult.
    // }
}
