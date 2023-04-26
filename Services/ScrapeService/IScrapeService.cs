using Models.DomainModels;

namespace Services.ScrapeService;

public interface IScrapeService
{
    public Task<YoutubeVideo?> ScrapeYoutubeVideo(string videoId);
    public Task<string> GetRawHtml(string url);

    public Task<YoutubeVideo[]> ScrapeYoutubeSearchResults(string searchQuery, int maxResults = 20);

    // public Task<YoutubeVideo[]> ScrapeYoutubeChannel(string url);
    public Task<YoutubeChannel> ScrapeChannelById(string channelId, int maxResults = 20);
    public Task<YoutubeVideo[]> ScrapeChannelByHandle(string userName);

    public Task<YoutubeVideo[]> ScrapeHashtag(string tag);

    public Task<YoutubePlaylist> ScrapePlaylist(string playlistId);


    public Task DownloadChannelThumbnail(string channelId);
}
