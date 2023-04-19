using System.Diagnostics;
using Models.DomainModels;
using Models.Factories;
using YoutubeExplode;
using YoutubeExplode.Channels;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Services.YoutubeExplodeService;

public class YoutubeExplodeService : IYoutubeExplodeService
{
    private readonly YoutubeClient _youtube;

    public YoutubeExplodeService()
    {
        _youtube = new YoutubeClient();
    }


    public async Task<IChannel> GetChannel(string channelId)
    {
        return null!;
        Channel chan = await _youtube.Channels.GetAsync(channelId);


        var url = chan.Thumbnails.First().Url;
        using var client = new HttpClient();

        var res = await client.GetAsync(url);
        var bytes = await res.Content.ReadAsByteArrayAsync();

        await File.WriteAllBytesAsync($"data/thumbnails/{chan.Id}.jpg", bytes);

        return chan;
    }

}
