using Models.DomainModels;
using YoutubeExplode.Common;
using YoutubeExplode.Search;

namespace Models.Factories;

public static class YoutubeSearchResultFactory
{
    public static YoutubeSearchResult FromVideoSearchResult(VideoSearchResult videoSearchResult)
    {
        Thumbnail thumbnail = videoSearchResult.Thumbnails.GetWithHighestResolution();
        return new YoutubeSearchResult
        {
            Id = videoSearchResult.Id,
            Title = videoSearchResult.Title,
            Duration = videoSearchResult.Duration ?? new TimeSpan(),
            ThumbnailUrl = thumbnail.Url,
            ChannelId = videoSearchResult.Author.ChannelId,
            ChannelTitle = videoSearchResult.Author.ChannelTitle,
        };
    }
}
