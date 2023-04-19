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

    public async Task<IStreamInfo[]> DownloadVideo(string videoId)
    {
        Video video = await _youtube.Videos.GetAsync(videoId);
        StreamManifest streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoId);

        IStreamInfo audioStreamInfo = streamManifest.GetAudioStreams().GetWithHighestBitrate();
        IVideoStreamInfo videoStreamInfo = streamManifest.GetVideoStreams().GetWithHighestVideoQuality();

        var streamInfos = new[] {audioStreamInfo, videoStreamInfo};

        await _youtube.Videos.DownloadAsync(streamInfos,
            new ConversionRequestBuilder($"Downloads/{video.Id}.{videoStreamInfo.Container}").Build());
        return streamInfos;
    }

    public async Task<IChannel> GetChannel(string channelId)
    {
        Channel chan = await _youtube.Channels.GetAsync(channelId);


        var url = chan.Thumbnails.First().Url;
        using var client = new HttpClient();

        var res = await client.GetAsync(url);
        var bytes = await res.Content.ReadAsByteArrayAsync();

        await File.WriteAllBytesAsync($"data/thumbnails/{chan.Id}.jpg", bytes);

        return chan;
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
        // Stopwatch stopwatch = new();
        // stopwatch.Start();

        Video video = await _youtube.Videos.GetAsync(url);

        // Console.WriteLine("Got video in " + stopwatch.ElapsedMilliseconds);
        // Task.Run(() => GetChannel(video.Author.ChannelId));
        // Console.WriteLine("Got channel in " + stopwatch.ElapsedMilliseconds);
        //
        // // var channel = await GetChannel(video.Author.ChannelId);

        var result = YoutubeVideoFactory.FromYoutubeExplode(video);
        // Console.WriteLine("conversion in " + stopwatch.ElapsedMilliseconds);
        // stopwatch.Stop();
        return result;
    }

    public async Task<IEnumerable<YoutubeVideo>> GetSearchResults(string query)
    {
        List<YoutubeSearchResult> results = new();

        Stopwatch stopwatch = new();
        stopwatch.Start();

        await foreach (var batch in _youtube.Search.GetResultBatchesAsync(query, SearchFilter.Video))
        {
            foreach (ISearchResult searchResult in batch.Items)
            {
                if (results.Count >= 12) break;
                Console.WriteLine(stopwatch.ElapsedMilliseconds);
                if (searchResult is not VideoSearchResult videoResult) continue;
                results.Add(YoutubeSearchResultFactory.FromVideoSearchResult(videoResult));
            }

            break;
        }

        var tasks = new List<Task<YoutubeVideo>>();

        foreach (var res in results)
        {
            tasks.Add(GetVideo(res.Id));
        }

        var responses = await Task.WhenAll(tasks);


        stopwatch.Stop();
        return responses;
    }
}
