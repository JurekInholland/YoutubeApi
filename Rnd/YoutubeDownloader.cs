using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Models;

namespace Rnd;

public static class YoutubeDownloader
{
    private static readonly Regex VideoIdRegex = new(@"(?:[?&]v=|\/embed\/|\/1\/|\/v\/|https:\/\/(?:www\.)?youtu\.be\/)([^&\n?#]+)");

    private static readonly Regex DonwloadProgressRegex =
        new(
            @"\[(?<kind>\w+)\]\s+(?<progress>\d+\.\d+)%\s+of\s+~\s+(?<size>\d+\.\d+)\s*(?<unit>[a-zA-Z]+)\s+at\s+(?<speed>\d+\.\d+)\s*(?<speedUnit>[a-zA-Z]+\/s)\s+ETA\s+(?<eta>\d{2}:\d{2})\s+\(frag\s+(?<frag>\d+\/\d+)\)");
    private static DateTime lastExecution = DateTime.Now;

    public delegate void DownloadProgressCallback(string? progress, string videoId, DateTime timeSinceLastExecution);

    public static string ParseVideoId(string routeArgs)
    {
        var match = VideoIdRegex.Match(routeArgs);
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return string.Empty;
    }

    public static DownloadProgress? ParseProgressLine(string line, string videoId)
    {
        if (line is null or "")
        {
            return null;
        }

        var match = DonwloadProgressRegex.Match(line);
        if (match.Success)
        {
            string kind = match.Groups["kind"].Value;
            float progress = float.Parse(match.Groups["progress"].Value, CultureInfo.InvariantCulture);
            float size = float.Parse(match.Groups["size"].Value);
            string unit = match.Groups["unit"].Value;
            float speed = float.Parse(match.Groups["speed"].Value.Replace('.', ','));
            string speedUnit = match.Groups["speedUnit"].Value;
            TimeSpan eta = TimeSpan.ParseExact(match.Groups["eta"].Value, "mm':'ss", CultureInfo.InvariantCulture);
            string frag = match.Groups["frag"].Value;


            return new DownloadProgress
            {
                Id = videoId,
                Status = kind,
                Progress = progress,
                Speed = speed + speedUnit,
                Eta = eta,
                Fragment = frag
            };
        }

        return null;
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

        lastExecution = DateTime.Now;
        var prog = ParseProgressLine(progress, videoId);
        if (prog is not null)
            Console.WriteLine(prog.ToString());
    }

    public static async Task<Stream> DownloadVideo(string videoId,
        DownloadProgressCallback progressCallback = null!)
    {
        var cmd =
            $"yt-dlp --merge-output-format \"mkv\" --embed-metadata -f bestvideo+bestaudio {videoId} -o \"data/%(uploader)s/%(title)s.%(ext)s\"";


        var stream = new MemoryStream();

        // var delta = 0;



        await CliCommand.CallCommand(cmd,
            (_, args) => progressCallback(args.Data, videoId, DateTime.Now));

        return stream;
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
