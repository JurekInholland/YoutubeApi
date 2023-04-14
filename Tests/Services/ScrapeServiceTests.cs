using Microsoft.Extensions.Logging.Abstractions;
using Models.DomainModels;
using Moq;
using Services.ScrapeService;

namespace Tests.Services;

public class ScrapeServiceTests
{
    private ScrapeService _scraper = null!;

    [SetUp]
    public void Setup()
    {
        var logger = new NullLogger<ScrapeService>();
        var clientFactoryMock = new Mock<IHttpClientFactory>(MockBehavior.Strict);
        clientFactoryMock.Setup(x => x.CreateClient()).Returns(new HttpClient());

        _scraper = new ScrapeService(logger, clientFactoryMock.Object);
    }

    /// <summary>
    /// Verify that the custom youtube scraper returns a youtube video with
    /// all values populated
    /// </summary>
    [Test]
    public async Task ScrapeYoutubeVideo_ReturnsVideo()
    {
        const string videoId = "dQw4w9WgXcQ";

        YoutubeVideo? video = await _scraper.ScrapeYoutubeVideo(videoId);
        Assert.Multiple(() =>
        {
            Assert.That(video, Is.Not.Null);
            Assert.That(video!.Id, Is.EqualTo(videoId));
            Assert.That(video.Title, Is.EqualTo("Rick Astley - Never Gonna Give You Up (Official Music Video)"));
            Assert.That(video.Description, Does.StartWith("The official video for “Never Gonna Give You Up” by Rick Astley"));
            Assert.That(video.Duration, Is.EqualTo(TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(32))));
            Assert.That(video.LikeCount, Is.GreaterThanOrEqualTo(1000000));
            Assert.That(video.ViewCount, Is.GreaterThanOrEqualTo(10000000));
            Assert.That(video.UploadDate, Is.EqualTo(new DateTime(2009, 10, 24)));
            Assert.That(video.Categories, Has.Length.GreaterThanOrEqualTo(2));
            Assert.That(video.RelatedVideos, Has.Length.GreaterThanOrEqualTo(20));

            Assert.That(video.YoutubeChannel.Id, Is.EqualTo("UCuAXFkgsw1L7xaCfnd5JJOw"));
            Assert.That(video.YoutubeChannel.Title, Is.EqualTo("Rick Astley"));
            Assert.That(video.YoutubeChannel.Subscribers, Does.EndWith("M"));
        });
    }
}
