using YoutubeExplode.Common;
using YoutubeExplode.Videos;

namespace Services.Extensions;

public static class VideoExtensions
{
    public static Thumbnail GetBestThumbnail(this Video video)
    {
        return video.Thumbnails.OrderByDescending(x => x.Resolution).First();
    }
}
