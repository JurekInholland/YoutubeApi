using Models;
using Models.Factories;
using YoutubeExplode;
using YoutubeExplode.Channels;
using YoutubeExplode.Common;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;
using YoutubeExplode.Videos.Streams;

namespace Services.YoutubeExploseService;

public class YoutubeExplodeService : IYoutubeExplodeService
{
    private readonly YoutubeClient _youtube;

    public YoutubeExplodeService()
    {
        _youtube = new YoutubeClient();
    }

    public async Task<IStreamInfo[]> DownloadVideo(string videoId)
    {
        var video = await _youtube.Videos.GetAsync(videoId);
        var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId);

        var audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
        var videoStreamInfo = streamManifest.GetVideoStreams().GetWithHighestVideoQuality();

        var streamInfos = new[] {audioStreamInfo, videoStreamInfo};

        // var stream = await _youtube.Videos.Streams.GetAsync(streamInfo);

        await _youtube.Videos.DownloadAsync(streamInfos,
            new ConversionRequestBuilder($"Downloads/{video.Id}.{videoStreamInfo.Container}").Build());
        return streamInfos;
    }

    public async Task<IChannel> GetChannel(string channelId)
    {
        return await _youtube.Channels.GetAsync(channelId);
    }


    public async Task<Video[]> GetRelatedVideos(string videoId)
    {
        var video = await _youtube.Videos.GetAsync(videoId);
        List<Video> results = new();
        await foreach (var batch in _youtube.Search.GetResultBatchesAsync(video.Title))
        {
            foreach (var result in batch.Items)
            {
                if (results.Count >= 12) break;
                if (result is VideoSearchResult videoResult)
                {
                    Console.WriteLine("Fetching: " + videoResult.Id);
                    var relatedVideo = await _youtube.Videos.GetAsync(videoResult.Id);
                    results.Add(relatedVideo);
                }
            }

            if (results.Count >= 12) break;
        }

        return results.ToArray();
    }

    public async Task<YoutubeVideo> GetVideo(string url)
    {
        var video = await _youtube.Videos.GetAsync(url);

        return YoutubeVideoFactory.FromYoutubeExplode(video);
    }
}
