using Models.DomainModels;

namespace Services.ScrapeService;

public interface IScrapeService
{
    public Task<YoutubeVideo?> ScrapeYoutubeVideo(string videoId);
    public Task<string> GetRawHtml(string url);
    public Task<YoutubeVideo[]> ScrapeYoutubeSearchResults(string searchQuery);
    public Task<YoutubeVideo?[]> ScrapeYoutubeChannel(string channelId);
}
