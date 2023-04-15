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

    public async Task<IEnumerable<YoutubeVideo>> GetLocalVideos()
    {
        return await _unitOfWork.YoutubeVideos.All().Where(y => y.LocalVideo != null)
            .Include(y => y.LocalVideo)
            .Include(y => y.YoutubeChannel)
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
