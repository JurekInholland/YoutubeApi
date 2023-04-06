using Rnd;

Console.WriteLine("Hello, World!");


var id = YoutubeDownloader.ParseVideoId("https://www.youtube.com/watch?v=QH2-TGUlwu4");


var info = await YoutubeDownloader.GetVideoInfo(id);

Console.WriteLine(info);
