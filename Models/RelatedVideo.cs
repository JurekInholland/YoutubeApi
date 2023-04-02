using YoutubeExplode.Search;

namespace Models;

public class RelatedVideo : BaseEntity
{
    public string Title { get; set; }
    public string ChannelId { get; set; }
    public string ChannelTitle { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public DateTime PublishTime { get; set; }
    public int ViewCount { get; set; }
    public int LikeCount { get; set; }
    public string[] Tags { get; set; }


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
