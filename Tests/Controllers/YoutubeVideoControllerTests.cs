using App.Controllers;
using Domain.Context;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Models.DomainModels;
using Moq;
using Services.YoutubeExplodeService;

namespace Tests.Controllers;

public class YoutubeVideoControllerTests
{
    private NullLogger<YoutubeVideoController> _logger = null!;
    private IUnitOfWork _unitOfWorkMock = null!;
    private Mock<IYoutubeExplodeService> _youtubeExplodeServiceMock = null!;
    private YoutubeVideoController _youtubeVideoController = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<YoutubeAppContext>()
            .UseInMemoryDatabase(databaseName: "youtube-tests")
            .Options;

        _logger = new NullLogger<YoutubeVideoController>();
        _youtubeExplodeServiceMock = new Mock<IYoutubeExplodeService>();
        // _controller = new YoutubeExplodeController(_logger, _youtubeExplodeServiceMock.Object);
        _unitOfWorkMock = new UnitOfWork(new YoutubeAppContext(options));

        _youtubeVideoController = new YoutubeVideoController(_logger, _unitOfWorkMock, _youtubeExplodeServiceMock.Object);
    }


    [Test]
    public async Task GetYoutubeVideo_WithExistingVideo_ReturnsVideo()
    {
        // Arrange
        var videoId = "dQw4w9WgXcQ";
        var video = new YoutubeVideo {Id = videoId};
        // _youtubeExplodeServiceMock.Setup(svc => svc.GetVideo(videoId)).ReturnsAsync(video);

        // Act
        var result = await _youtubeVideoController.GetYoutubeVideo(videoId);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult) result;
        Assert.That(okResult.Value!, Is.EqualTo(video));
    }
}
