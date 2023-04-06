using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Models.DomainModels;

namespace Services.ScrapeService;

public partial class ScrapeService : IScrapeService
{
    private readonly HttpClient _httpClient;

    public ScrapeService()
    {
        _httpClient = new HttpClient();
    }

    public static long ConvertNumberStringToLong(string numberString)
    {
        var suffixes = new Dictionary<char, long>
        {
            {'K', 1000},
            {'M', 1000000},
            {'B', 1000000000},
            {'T', 1000000000000}
        };

        char suffix = numberString.Last();
        long multiplier = 1;
        if (suffixes.TryGetValue(suffix, out var suffix1))
        {
            multiplier = suffix1;
            numberString = numberString[..^1];
        }

        if (!double.TryParse(numberString, out double number))
        {
            throw new ArgumentException("Invalid number string format");
        }

        return (long) (number * multiplier);
    }

    [GeneratedRegex("<title>([^&]+) - YouTube</title>")]
    private static partial Regex VideoTitle();

    [GeneratedRegex("""description":{"simpleText":"([^"]+)""")]
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

    // [GeneratedRegex("""owner":{"videoOwnerRenderer":{"thumbnail":{"thumbnails":\[{"url":"(.*?)""")]
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

    public async Task<string> GetRawHtml(string url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        string sourceCode = htmlDocument.DocumentNode.OuterHtml;
        return sourceCode;
    }

    public async Task<YoutubeVideo> ScrapeYoutubeVideo(string videoId)
    {
        var url = $"https://www.youtube.com/watch?v={videoId}&hl=en";
        var html = await _httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);
        string sourceCode = htmlDocument.DocumentNode.OuterHtml;

        string title = VideoTitle().Match(sourceCode).Groups[1].Value;
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
        string[] relatedVideos = RelatedVideos().Matches(sourceCode).Select(m => m.Groups[1].Value).ToHashSet().ToArray();

        string channelName = YtChannelName().Match(sourceCode).Groups[1].Value;

        if (likeCount == "No") likeCount = "0";

        YoutubeVideo scrapedVideo = new()
        {
            Id = videoId,
            Title = title,
            Description = description,
            DateAdded = DateTime.Now,
            LastUpdated = DateTime.Now,
            UploadDate = DateTime.Parse(uploadDate),
            Duration = TimeSpan.FromSeconds(int.Parse(lengthSeconds)),
            WebpageUrl = url,
            ViewCount = int.Parse(viewCount),
            LikeCount = int.Parse(likeCount, NumberStyles.AllowThousands, CultureInfo.InvariantCulture),
            Categories = keywords,
            Comments = null!,
            RelatedVideos = relatedVideos.Skip(1).ToArray(),
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
}
