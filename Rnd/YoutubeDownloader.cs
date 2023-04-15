using System.Text.RegularExpressions;

namespace Rnd;

public static class YoutubeDownloader
{
    private static readonly Regex VideoIdRegex = new(@"(?:[?&]v=|\/embed\/|\/1\/|\/v\/|https:\/\/(?:www\.)?youtu\.be\/)([^&\n?#]+)");

    /// <summary>
    /// Callback delegate; Pass a method with this signature to the DownloadVideo method to get progress updates
    /// </summary>
    public delegate Task DownloadProgressCallback(string? progress, string videoId);

    public static string ParseVideoId(string routeArgs)
    {
        var match = VideoIdRegex.Match(routeArgs);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return string.Empty;
    }

    public static async Task DownloadVideo(string videoId, string outputFormat,
        DownloadProgressCallback progressCallback = null!)
    {
        Console.WriteLine("DownloadVideo: " + videoId);
        var cmd =
            @$"yt-dlp --merge-output-format ""mkv"" --embed-metadata --write-info-json -f bestvideo+bestaudio -o '{outputFormat}' -- {videoId}";


        await CliCommand.CallCommand(cmd,
            (_, args) => progressCallback(args.Data, videoId));
    }

    public static async Task<string> GetChannelInfo(string channelId)
    {
        var cmd = $@"yt-dlp -j https://www.youtube.com/channel/{channelId}";
        await CliCommand.CallCommand(cmd);
        return "dbg";
    }

    public static async Task<string> GetVideoInfo(string videoId)
    {
        var cmd = $@"yt-dlp -j {videoId}";
        await CliCommand.CallCommand(cmd);
        return "info";
    }
}
