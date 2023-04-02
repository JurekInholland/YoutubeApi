using Models;
using Models.DomainModels;
using YoutubeExplode.Channels;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Services.YoutubeExplodeService;

public interface IYoutubeExplodeService
{
    public Task<YoutubeVideo> GetVideo(string url);
    public Task<Video[]> GetRelatedVideos(string videoId);
    public Task<IStreamInfo[]> DownloadVideo(string videoId);

    public Task<IChannel> GetChannel(string channelId);

    public Task<IEnumerable<YoutubeSearchResult>> GetSearchResults(string query);
}
