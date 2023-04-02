using Models;
using YoutubeExplode.Channels;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Services.YoutubeExploseService;

public interface IYoutubeExplodeService
{
    public Task<YoutubeVideo> GetVideo(string url);
    public Task<Video[]> GetRelatedVideos(string videoId);
    public Task<IStreamInfo[]> DownloadVideo(string videoId);

    public Task<IChannel> GetChannel(string channelId);
}
