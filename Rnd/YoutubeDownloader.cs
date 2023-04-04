using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Models;

namespace Rnd;

public static class YoutubeDownloader
{
    private static readonly Regex VideoIdRegex = new(@"(?:[?&]v=|\/embed\/|\/1\/|\/v\/|https:\/\/(?:www\.)?youtu\.be\/)([^&\n?#]+)");

    // private static DateTime lastExecution = DateTime.Now;

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


    private static void ProcessDownloadProgress(string? progress, string videoId, DateTime lastExecution)
    {
        // Console.WriteLine("time: " + TimeSpan.FromSeconds(.1));

        if (progress is null) return;

        if (progress.Contains("[Metadata] Adding metadata to"))
        {
            Console.WriteLine("DONE!!!");
            return;
        }

        // if (delta < TimeSpan.FromSeconds(.2))
        // {
        //     // Console.WriteLine("[SKIPPING] " + progress);
        //     return;
        // }

        // lastExecution = DateTime.Now;
        // var prog = ParseProgressLine(progress, videoId);
        // if (prog is not null)
        //     Console.WriteLine(prog.ToString());
    }

    public static async Task DownloadVideo(string videoId,
        DownloadProgressCallback progressCallback = null!)
    {
        Console.WriteLine("DownloadVideo: " + videoId);
        var cmd =
            $"yt-dlp --merge-output-format \"mkv\" --embed-metadata -f bestvideo+bestaudio {videoId} -o \"data/%(uploader)s/%(title)s.%(ext)s\"";


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
