using App.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Models;
using Models.DomainModels;
using Moq;
using Services.YoutubeExplodeService;

namespace Tests.Controllers;

public class YoutubeExplodeControllerTests
{
    private ILogger<YoutubeExplodeController> _logger = null!;

    private Mock<IYoutubeExplodeService> _youtubeExplodeServiceMock = null!;
    private YoutubeExplodeController _controller = null!;

    [SetUp]
    public void Setup()
    {
        _logger = new NullLogger<YoutubeExplodeController>();

        _youtubeExplodeServiceMock = new Mock<IYoutubeExplodeService>();
        _controller = new YoutubeExplodeController(_logger, _youtubeExplodeServiceMock.Object);
    }

    [Test]
    public async Task GetVideoInfo_ValidVideoId_ReturnsOkObjectResult()
    {
        // Arrange
        var videoId = "abc123";
        var video = new YoutubeVideo {Id = videoId, Title = "Test Video"};
        _youtubeExplodeServiceMock.Setup(svc => svc.GetVideo(videoId)).ReturnsAsync(video);

        // Act
        IActionResult result = await _controller.GetVideoInfo(videoId);

        // Assert
        var okResult = result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        var returnedVideo = okResult?.Value as YoutubeVideo;
        Assert.That(returnedVideo, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(returnedVideo?.Id, Is.EqualTo(video.Id));
            Assert.That(returnedVideo?.Title, Is.EqualTo(video.Title));
        });
    }
}
