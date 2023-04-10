using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Models.DomainModels;

namespace Services.ScrapeService;

public partial class ScrapeService : IScrapeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ScrapeService> _logger;

    public ScrapeService(ILogger<ScrapeService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    [GeneratedRegex(@"description"":\{""simpleText"":""([^&]+(?:&quot;[^&]+)*|[^&]+(?:""[^""]+)*?)""\},""lengthSeconds")]
    private static partial Regex Description();

    [GeneratedRegex("""lengthSeconds":"([^"]+)""")]
    private static partial Regex LengthSeconds();

    [GeneratedRegex("""ownerProfileUrl":"([^"]+)""")]
    private static partial Regex OwnerProfile();

    [GeneratedRegex("""externalChannelId":"([^"]+)""")]
    private static partial Regex ChannelId();

    [GeneratedRegex("""viewCount":"([^"]+)""")]
    private static partial Regex ViewCount();

    [GeneratedRegex("""subscriberCountText":{"accessibility":{"accessibilityData":{"label":"([^"]+)"}},"simpleText":"([^"]+)"}""")]
    private static partial Regex FollowerCount();

    [GeneratedRegex("""accessibilityData":{"label":"([^"]+) likes"}""")]
    private static partial Regex LikeCount();

    [GeneratedRegex(@"""owner"":\{""videoOwnerRenderer"":\{""thumbnail"":\{""thumbnails"":\[\{""url"":""(.*?)""")]
    private static partial Regex AvatarUrl();

    [GeneratedRegex("""uploadDate":"(?<date>\d{4}-\d{2}-\d{2})""")]
    private static partial Regex UploadDate();

    [GeneratedRegex("""<meta name="keywords" content="(?<keywords>[^"]+)">""")]
    private static partial Regex Keywords();

    [GeneratedRegex("""videoId":"([^"]+)""")]
    private static partial Regex RelatedVideos();

    [GeneratedRegex("""<link itemprop="name" content="([^"]+)""")]
    private static partial Regex YtChannelName();


    [GeneratedRegex("\"bitrate\":\\d+,\"width\":(\\d+),\"height\":(\\d+),\"initRange\"")]
    private static partial Regex VideoResolution();

    public async Task<string> GetRawHtml(string url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        string sourceCode = htmlDocument.DocumentNode.OuterHtml;
        return sourceCode;
    }

    private async Task<HtmlDocument> GetHtmlDocument(string url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        return htmlDocument;
    }

    public async Task<YoutubeVideo?[]> ScrapeYoutubeChannel(string channelId)
    {
        var url = $"https://www.youtube.com/channel/{channelId}";
        HtmlDocument document = await GetHtmlDocument(url);
        string sourceCode = document.DocumentNode.OuterHtml;
        Regex regex = new Regex(@"(?<=watch\?v=)[\w-]+");
        MatchCollection matches = regex.Matches(sourceCode);

        var videoIds = matches.Select(m => m.Value).ToHashSet().ToArray();
        Task<YoutubeVideo?>[] tasks = new Task<YoutubeVideo?>[videoIds.Length];
        for (int i = 0; i < videoIds.Length; i++)
        {
            tasks[i] = ScrapeYoutubeVideo(videoIds[i]);
        }

        var res = await Task.WhenAll(tasks);
        return res;
    }


    public async Task<YoutubeVideo[]> ScrapeYoutubeSearchResults(string searchQuery)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        var url = $"https://www.youtube.com/results?search_query={searchQuery}&hl=en";
        HtmlDocument document = await GetHtmlDocument(url);
        Console.WriteLine($"GetHtmlDocument: {stopwatch.ElapsedMilliseconds}ms");

        string sourceCode = document.DocumentNode.OuterHtml;

        Regex regex = new Regex(@"(?<=watch\?v=)[\w-]+");
        MatchCollection matches = regex.Matches(sourceCode);

        var videoIds = matches.Select(m => m.Value).ToHashSet().Take(12).ToArray();
        Console.WriteLine($"Regex: {stopwatch.ElapsedMilliseconds}ms");
        Task<YoutubeVideo?>[] tasks = new Task<YoutubeVideo?>[videoIds.Length];
        for (int i = 0; i < videoIds.Length; i++)
        {
            tasks[i] = ScrapeYoutubeVideo(videoIds[i]);
        }

        var result = await Task.WhenAll(tasks);
        Console.WriteLine($"ScrapeYoutubeVideo: {stopwatch.ElapsedMilliseconds}ms");


        return result.Where(x => x != null).Select(x => x!).ToArray();
    }

    public async Task<YoutubeVideo?> ScrapeYoutubeVideo(string videoId)
    {
        var url = $"https://www.youtube.com/watch?v={videoId}&hl=en";
        HtmlDocument document;
        try
        {
            document = await GetHtmlDocument(url);
        }
        catch (Exception e)
        {
            return null;
        }

        string sourceCode = document.DocumentNode.OuterHtml;

        string title = document.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim().Replace(" - YouTube", "") ?? "Not found";

        string description = Description().Match(sourceCode).Groups[1].Value;
        string lengthSeconds = LengthSeconds().Match(sourceCode).Groups[1].Value;
        string ownerProfileUrl = OwnerProfile().Match(sourceCode).Groups[1].Value;
        string externalChannelId = ChannelId().Match(sourceCode).Groups[1].Value;
        string viewCount = ViewCount().Match(sourceCode).Groups[1].Value;
        string followerCount = FollowerCount().Match(sourceCode).Groups[2].Value.Replace(" subscribers", "");
        string likeCount = LikeCount().Match(sourceCode).Groups[1].Value;
        string avatarUrl = AvatarUrl().Match(sourceCode).Groups[1].Value;
        string uploadDate = UploadDate().Match(sourceCode).Groups[1].Value;
        string[] keywords = Keywords().Match(sourceCode).Groups[1].Value.Split(", ");
        string[] relatedVideoIds = RelatedVideos().Matches(sourceCode).Select(m => m.Groups[1].Value).ToHashSet().ToArray();

        string width = VideoResolution().Match(sourceCode).Groups[1].Value;
        string height = VideoResolution().Match(sourceCode).Groups[2].Value;


        string channelName = YtChannelName().Match(sourceCode).Groups[1].Value;

        if (likeCount is "No" or "") likeCount = "0";

        try
        {
            YoutubeVideo scrapedVideo = new()
            {
                Id = videoId,
                Title = title,
                Description = description,
                DateAdded = DateTime.Now,
                LastUpdated = DateTime.Now,
                UploadDate = DateTime.Parse(uploadDate),
                Width = int.Parse(width),
                Height = int.Parse(height),
                Duration = TimeSpan.FromSeconds(int.Parse(lengthSeconds)),
                WebpageUrl = url,
                ViewCount = long.Parse(viewCount),
                LikeCount = long.Parse(likeCount, NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
                Categories = keywords,
                Comments = null!,
                RelatedVideos = relatedVideoIds.Skip(1).ToArray(),
                YoutubeChannel = new YoutubeChannel
                {
                    Id = externalChannelId,
                    Title = channelName,
                    Handle = ownerProfileUrl.Replace("http://www.youtube.com/", ""),
                    ThumbnailUrl = avatarUrl,
                    Subscribers = followerCount,
                    Videos = null!
                }
            };
            return scrapedVideo;
        }
        catch (Exception e)
        {
            Console.WriteLine("EXCEPTION: " + e.Message);
            return null;
        }
    }
}
