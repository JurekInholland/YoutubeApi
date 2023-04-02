using System.Diagnostics;
using Models;
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

    public async Task<IStreamInfo[]> DownloadVideo(string videoId)
    {
        Video video = await _youtube.Videos.GetAsync(videoId);
        StreamManifest streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId);

        IStreamInfo audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
        IVideoStreamInfo videoStreamInfo = streamManifest.GetVideoStreams().GetWithHighestVideoQuality();

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
        Video video = await _youtube.Videos.GetAsync(videoId);
        List<Video> results = new();
        await foreach (var batch in _youtube.Search.GetResultBatchesAsync(video.Title))
        {
            foreach (ISearchResult result in batch.Items)
            {
                if (results.Count >= 12) break;
                if (result is not VideoSearchResult videoResult) continue;

                Console.WriteLine("Fetching: " + videoResult.Id);
                Video relatedVideo = await _youtube.Videos.GetAsync(videoResult.Id);
                results.Add(relatedVideo);
            }

            if (results.Count >= 12) break;
        }

        return results.ToArray();
    }

    public async Task<YoutubeVideo> GetVideo(string url)
    {
        Video video = await _youtube.Videos.GetAsync(url);
        return YoutubeVideoFactory.FromYoutubeExplode(video);
    }

    public async Task<IEnumerable<YoutubeSearchResult>> GetSearchResults(string query)
    {
        List<YoutubeSearchResult> results = new();

        Stopwatch stopwatch = new();
        stopwatch.Start();

        await foreach (var batch in _youtube.Search.GetResultBatchesAsync(query))
        {
            foreach (ISearchResult searchResult in batch.Items)
            {
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                if (searchResult is not VideoSearchResult videoResult) continue;
                results.Add(YoutubeSearchResultFactory.FromVideoSearchResult(videoResult));
            }

            break;
        }

        stopwatch.Stop();
        return results;
        // foreach (var item in results)
        // {
        //     if (item is not VideoSearchResult videoResult) continue;
        //     Console.WriteLine("Fetching: " + videoResult.Id);
        //     Video video = await _youtube.Videos.GetAsync(videoResult.Id);
        //     results.Add(video);
        // }
    }
}
