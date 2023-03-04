using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Rnd;

namespace Services.YoutubeService;

public class YoutubeService : IYoutubeService
{
    private readonly ILogger<YoutubeService> _logger;
    private readonly AppConfig _config;

    private readonly string _youtubeDlPath;

    private string DownloadCommand => $@"{_youtubeDlPath} -j -N 5 --buffer-size 16k --write-subs --add-chapters --simulate";
    private string OutputFormat => $@"--output $(id) $(title) $(ext)";
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public YoutubeService(ILogger<YoutubeService> logger, IOptions<AppConfig> config)
    {
        _logger = logger;
        _config = config.Value;
        _youtubeDlPath = _config.YtDlPath;
    }

    /// <summary>
    /// Make a GET request to validate the given Id is a valid Youtube video Id.
    /// https: //webapps.stackexchange.com/a/54448
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> IsValidId(string id)
    {
        using var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Head,
            $"https://www.youtube.com/oembed?url=http://www.youtube.com/watch?v={id}&format=json");
        var response = await client.SendAsync(request);

        // var response = await client.GetAsync($"https://gdata.youtube.com/feeds/api/videos/{id}");
        // var response = await client.GetAsync($"https://www.youtube.com/watch?v={id}&format=json");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DownloadVideo(string id)
    {
        if (!await IsValidId(id)) return false;
        var cmd = $@"{_youtubeDlPath} -j {id}";
        var res = await CliCommand.CallCommand(cmd);
        return true;
    }

    public async Task<YoutubeVideo?> GetVideoInfo(string id)
    {
        if (!await IsValidId(id)) return null;

        var cmd = $@"{_youtubeDlPath} -j {id}";
        var res = await CliCommand.CallCommand(cmd);
        var video = JsonSerializer.Deserialize<YoutubeVideo>(res, _serializerOptions);
        return video;
    }

    public async Task<dynamic?> GetFullInfo(string id)
    {
        if (!await IsValidId(id)) throw new InvalidDataException("Invalid videoId");
        var cmd = $@"{_youtubeDlPath} -j {id}";
        var res = await CliCommand.CallCommand(cmd);
        dynamic? json = JsonSerializer.Deserialize<dynamic>(res, _serializerOptions);
        return json;
    }


    public async Task<string> GetChannelInfo(string id)
    {
        var cmd = $@"{_youtubeDlPath} -j https://www.youtube.com/channel/{id}";
        var res = await CliCommand.CallCommand(cmd);

        return res;
    }
}
