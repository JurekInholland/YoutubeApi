using Models;
using Models.DomainModels;
using YoutubeExplode.Channels;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Services.YoutubeExplodeService;

public interface IYoutubeExplodeService
{
    public Task<IChannel> GetChannel(string channelId);
}
