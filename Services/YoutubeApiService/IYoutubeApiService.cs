using Models;

namespace Services.YoutubeApiService;

public interface IYoutubeApiService
{
    public Task<dynamic?> GetRelatedVideos(string videoId);
}
