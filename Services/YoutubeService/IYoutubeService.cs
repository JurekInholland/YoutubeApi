using Models;
using Models.DomainModels;

namespace Services.YoutubeService;

public interface IYoutubeService
{
    public Task<bool> IsValidId(string id);
    public Task<bool> DownloadVideo(string id);
    public Task<YoutubeVideo?> GetVideoInfo(string id);
    public Task<string> GetChannelInfo(string id);
    public Task<dynamic?> GetFullInfo(string id);

}
