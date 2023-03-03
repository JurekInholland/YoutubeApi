using System.Text.RegularExpressions;

namespace Rnd;

public static class YoutubeDownloader
{
    private static readonly Regex VideoIdRegex = new(@"(?:[?&]v=|\/embed\/|\/1\/|\/v\/|https:\/\/(?:www\.)?youtu\.be\/)([^&\n?#]+)");


    public static string ParseVideoId(string routeArgs)
    {
        var match = VideoIdRegex.Match(routeArgs);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return string.Empty;
    }

    public static async Task<string> DownloadVideo(string videoId)
    {
        var cmd = $"yt-dlp --merge-output-format \"mkv\" --embed-metadata -f bestvideo+bestaudio {videoId}";
        var res = await CliCommand.CallCommand(cmd);
        return res;
    }

    public static async Task<string> GetChannelInfo(string channelId)
    {
        var cmd = $@"yt-dlp -j https://www.youtube.com/channel/{channelId}";
        var res = await CliCommand.CallCommand(cmd);
        return res;
    }

    public static async Task<string> GetVideoInfo(string videoId)
    {
        var cmd = $@"yt-dlp -j {videoId}";
        var res = await CliCommand.CallCommand(cmd);
        return res;
    }
}
