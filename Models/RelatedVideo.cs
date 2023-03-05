namespace Models;

public class RelatedVideo : BaseEntity
{
    public string Title { get; set; }
    public string ChannelId { get; set; }
    public string ChannelTitle { get; set; }
    public string Description { get; set; }
    public string Thumbnail { get; set; }
    public DateTime PublishTime { get; set; }


}
