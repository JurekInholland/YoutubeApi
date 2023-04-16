using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.DomainModels;
using Rnd;

namespace Services.YoutubeService;

public class YoutubeService : IYoutubeService
{
    private readonly ILogger<YoutubeService> _logger;
    private readonly AppConfig _config;
    private readonly IUnitOfWork _unitOfWork;

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

    public YoutubeService(ILogger<YoutubeService> logger, IOptions<AppConfig> config, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _config = config.Value;
        _youtubeDlPath = _config.YtDlPath;
        _unitOfWork = unitOfWork;
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
        HttpResponseMessage response = await client.SendAsync(request);

        // var response = await client.GetAsync($"https://gdata.youtube.com/feeds/api/videos/{id}");
        // var response = await client.GetAsync($"https://www.youtube.com/watch?v={id}&format=json");
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DownloadVideo(string id)
    {
        if (!await IsValidId(id)) return false;
        var cmd = $@"{_youtubeDlPath} -j {id}";
        // var res = await CliCommand.CallCommand(cmd);
        return true;
    }

    public async Task<Stream> GetLocalVideo(string id)
    {
        var video = await _unitOfWork.YoutubeVideos.Where(x => x.Id == id).Include(v => v.LocalVideo).Include(v => v.YoutubeChannel)
            .FirstOrDefaultAsync();

        var path = Path.Combine("Downloads", $"{id}");

        return new FileStream(path, FileMode.Open);
    }

    public async Task<YoutubeVideo?> GetVideoInfo(string id)
    {
        if (!await IsValidId(id)) return null;

        var cmd = $@"{_youtubeDlPath} -j {id}";
        // var res = await CliCommand.CallCommand(cmd);
        // var video = JsonSerializer.Deserialize<YoutubeVideo>(res, _serializerOptions);
        return null;
    }

    public async Task<string> GetMetadata(string id)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        var cmd = $@"{_youtubeDlPath} --print channel_follower_count {id}";
        var res = await CliCommand.CallCommandWithReturn(cmd);
        Console.WriteLine(stopWatch.ElapsedMilliseconds);
        return res;
    }

    public async Task<IEnumerable<YoutubeVideo>> GetLocalVideos()
    {
        return await _unitOfWork.YoutubeVideos.All()
            .Include(y => y.LocalVideo)
            .Include(y => y.YoutubeChannel)
            .Where(y => y.LocalVideo != null)
            .ToListAsync();
    }


    public async Task<string?[]> GetSearchCompletion(string query)
    {
        using var client = new HttpClient();
        var url = $"https://suggestqueries.google.com/complete/search?client=youtube&ds=yt&alt=json&q={query}&hl=en";
        var res = (await client.GetStringAsync(url)).Replace("window.google.ac.h(", "")[..^1];
        var json = JsonDocument.Parse(res).RootElement;

        var result = json.EnumerateArray().ToArray()[1].EnumerateArray().Select(x => x[0].GetString()).ToArray();

        return result;
    }
}
