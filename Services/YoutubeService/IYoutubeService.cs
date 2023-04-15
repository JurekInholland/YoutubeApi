using Models;
using Models.DomainModels;

namespace Services.YoutubeService;

public interface IYoutubeService
{

    public Task<string?[]> GetSearchCompletion(string query);
    public Task<IEnumerable<YoutubeVideo>> GetLocalVideos();
}
