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

    [GeneratedRegex(@"""playabilityStatus"":{""status"":""([^""]+)"",""messages"":\[""([^""]+)""")]
    private static partial Regex PlayabilityStatus();

    [GeneratedRegex("(?<=watch\\?v=)[\\w-]+")]
    private static partial Regex VideoIdRegex();

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


    [GeneratedRegex("/maxresdefault.jpg\",\"width\":(\\d+),\"height\":(\\d+)}]},\"allowRatings\":")]
    private static partial Regex VideoResolution2();


    [GeneratedRegex(@"""playableInEmbed"":(true|false)")]
    private static partial Regex PlayableInEmbed();

    [GeneratedRegex(@"""isLiveContent"":(true|false)},")]
    private static partial Regex IsLiveContent();


    [GeneratedRegex(@"{""url"":""https://yt3.googleusercontent.com/([^""]+)"",""width"":(\d+),""height"":(\d+)}")]
    private static partial Regex ChannelImg();

    [GeneratedRegex("""videosCountText":{"runs":\[{"text":"([^"]+)"},{"text":"([^"]+)"}\]}""")]
    private static partial Regex ChannelVideosCount();


    public async Task<string> GetRawHtml(string url)
    {
        HtmlDocument htmlDocument = await GetHtmlDocument(url);
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

    public async Task<YoutubeVideo[]> ScrapeHashtag(string tag)
    {
        var url = $"https://www.youtube.com/hashtag/{tag}";
        HtmlDocument document = await GetHtmlDocument(url);
        string sourceCode = document.DocumentNode.OuterHtml;
        return await ScrapeVideos(sourceCode);
    }

    public async Task<YoutubeVideo[]> ScrapeChannelByHandle(string userName)
    {
        var url = $"https://www.youtube.com/@{userName}";
        HtmlDocument document = await GetHtmlDocument(url);
        string sourceCode = document.DocumentNode.OuterHtml;
        return await ScrapeVideos(sourceCode);
    }

    public async Task<YoutubeVideo[]> ScrapeChannelById(string channelId)
    {
        var url = $"https://www.youtube.com/channel/{channelId}";
        HtmlDocument document = await GetHtmlDocument(url);
        string sourceCode = document.DocumentNode.OuterHtml;
        await ScrapeChannelInfo(sourceCode);
        return await ScrapeVideos(sourceCode);
    }

    private async Task ScrapeChannelInfo(string sourceCode)
    {

        List<(int, int, string)> avatars = new();
        List<(int, int, string)> banners = new();


        MatchCollection matches = ChannelImg().Matches(sourceCode);

        foreach (GroupCollection match in matches.Select(match => match.Groups))
        {
            var imageUrl = match[1].Value;
            if (imageUrl.EndsWith("-no-nd-rj"))
            {
                banners.Add((int.Parse(match[2].Value), int.Parse(match[3].Value), imageUrl));
            }
            else
            {
                avatars.Add((int.Parse(match[2].Value), int.Parse(match[3].Value), imageUrl));
            }
        }

        var bestAva = avatars.OrderByDescending(b => b.Item1).ToArray()[1];
        var bestBan = banners.OrderByDescending(b => b.Item1).ToArray()[0];

        _logger.LogInformation(bestAva + " " + bestBan);

        Match count = ChannelVideosCount().Match(sourceCode);
        var c1 = count.Groups[1].Value;
        var c2 = count.Groups[2].Value;
        _logger.LogInformation(c1 + " " + c2);
        Console.WriteLine();
    }

    private async Task<YoutubeVideo[]> ScrapeVideos(string sourceCode, int maxResults = 20)
    {
        MatchCollection matches = VideoIdRegex().Matches(sourceCode);

        var videoIds = matches.Select(m => m.Value).ToHashSet().ToArray();
        var tasks = new Task<YoutubeVideo?>[Math.Min(maxResults, videoIds.Length)];
        for (int i = 0; i < Math.Min(maxResults, videoIds.Length); i++)
        {
            tasks[i] = ScrapeYoutubeVideo(videoIds[i]);
        }

        var res = await Task.WhenAll(tasks);


        return res.Select(y => y!).ToArray();
    }


    public async Task<YoutubeVideo[]> ScrapeYoutubeSearchResults(string searchQuery, int maxResults = 20)
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
        var tasks = new Task<YoutubeVideo?>[Math.Min(maxResults, videoIds.Length)];
        for (int i = 0; i < Math.Min(maxResults, videoIds.Length); i++)
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


        string width2 = VideoResolution2().Match(sourceCode).Groups[1].Value;
        string height2 = VideoResolution2().Match(sourceCode).Groups[2].Value;

        var isLiveContent = IsLiveContent().Match(sourceCode).Groups[1].Value;

        var playableInEmbed = PlayableInEmbed().Match(sourceCode).Groups[1].Value;

        if (isLiveContent == "true")
        {
            width = "1280";
            height = "720";
        }

        if (width == "" && width2 != "" || height == "" && height2 != "")
        {
            width = width2;
            height = height2;
        }

        string channelName = YtChannelName().Match(sourceCode).Groups[1].Value;

        string playabilityStatus = PlayabilityStatus().Match(sourceCode).Groups[1].Value;
        string playabilityStatusMessage = PlayabilityStatus().Match(sourceCode).Groups[2].Value;

        if (likeCount is "No" or "") likeCount = "0";

        if (playabilityStatus != "")
        {
            throw new ArgumentException(playabilityStatusMessage != "" ? playabilityStatusMessage : playabilityStatus);
        }

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
                playableInEmbed = playableInEmbed == "true",
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
