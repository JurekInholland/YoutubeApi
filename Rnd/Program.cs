using Rnd;

Console.WriteLine("Hello, World!");

// CliCommand.CallCommand(new[] {"yt-dlp https://www.youtube.com/watch?v=QH2-TGUlwu4"});

var id = YoutubeDownloader.ParseVideoId("https://www.youtube.com/watch?v=QH2-TGUlwu4");

// var output = await YoutubeDownloader.DownloadVideo(id);

var info = await YoutubeDownloader.GetVideoInfo(id);

Console.WriteLine(info);
