using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace Services.YoutubeApiService;

public class YoutubeApiService : IYoutubeApiService
{
    private readonly ILogger<YoutubeApiService> _logger;
    private readonly AppConfig _config;


    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public YoutubeApiService(ILogger<YoutubeApiService> logger, IOptions<AppConfig> config)
    {
        _logger = logger;
        _config = config.Value;
    }

    private string GetRelatedVideosUrl(string videoId) =>
        $"https://www.googleapis.com/youtube/v3/search?part=snippet&relatedToVideoId={videoId}&type=video&key={_config.YoutubeApiKey}&maxResults=30";


    public async Task<dynamic?> GetRelatedVideos(string videoId)
    {
        using var client = new HttpClient();
        var res = await client.GetAsync(GetRelatedVideosUrl(videoId));
        var json = await res.Content.ReadAsStringAsync();

        var ele = JsonSerializer.Deserialize<JsonElement>(json, _serializerOptions);
        var items = ele.GetProperty("items").EnumerateArray();

        List<RelatedVideo> videos = new();

        foreach (var element in items)
        {
            var parsed = element.GetProperty("snippet").Deserialize<RelatedVideo>(_serializerOptions);
            if (parsed is null) continue;

            var thumbnails = element.GetProperty("snippet").GetProperty("thumbnails");

            parsed.Thumbnail = GetThumbnailUrl(thumbnails);

            try
            {
                parsed.Id = element.GetProperty("id").GetProperty("videoId").GetString() ?? "error";
            } catch (Exception e)
            {
                _logger.LogError(e, "Error parsing video id");
            }
            videos.Add(parsed);
        }

        return videos;
    }

    private string GetThumbnailUrl(JsonElement thumbnailArray)
    {
        if (thumbnailArray.TryGetProperty("maxres", out var thumb))
        {
            return thumb.GetProperty("url").GetString() ?? "error";
        }

        if (thumbnailArray.TryGetProperty("standard", out thumb))
        {
            return thumb.GetProperty("url").GetString() ?? "error";
        }

        if (thumbnailArray.TryGetProperty("high", out thumb))
        {
            return thumb.GetProperty("url").GetString() ?? "error";
        }

        if (thumbnailArray.TryGetProperty("medium", out thumb))
        {
            return thumb.GetProperty("url").GetString() ?? "error";
        }

        if (thumbnailArray.TryGetProperty("default", out thumb))
        {
            return thumb.GetProperty("url").GetString() ?? "error";
        }

        return "error";
    }
}
