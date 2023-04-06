using Models.DomainModels;

namespace Services.ScrapeService;

public interface IScrapeService
{
    public Task<YoutubeVideo> ScrapeYoutubeVideo(string url);
    public Task<string> GetRawHtml(string url);
}
