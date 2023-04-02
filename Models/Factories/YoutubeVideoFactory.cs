using YoutubeExplode.Videos;

namespace Models.Factories;

public static class YoutubeVideoFactory
{
    public static YoutubeVideo FromYoutubeExplode(Video video)
    {
        YoutubeVideo youtubeVideo = new()
        {
            Id = video.Id.Value,
            Title = video.Title,
            ChannelId = video.Author.ChannelId.Value,
            ChannelUrl = video.Author.ChannelUrl,

            DateAdded = DateTime.Now,
            LastUpdated = DateTime.Now,
            UploadDate = video.UploadDate.DateTime,

            Description = video.Description,
            Duration = video.Duration!.Value.Seconds,
            DurationString = video.Duration!.Value.ToString(),
            Thumbnail = video.Thumbnails.OrderByDescending(x => x.Resolution.Area).First().Url,
            Uploader = video.Author.ChannelTitle,
            UploaderId = video.Author.ChannelId,
            ViewCount = video.Engagement.ViewCount,
            LikeCount = video.Engagement.LikeCount,
            Categories = video.Keywords.ToArray(),
            Width = 0,
            Height = 0,
            Fps = 0,
            FileSize = 0,
            Vbr = 0,
            Abr = 0,
            Comments = new(),
            WebpageUrl = video.Url,
        };
        return youtubeVideo;
    }
}
