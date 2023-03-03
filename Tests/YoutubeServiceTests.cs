using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Moq;
using Services.YoutubeService;

namespace Tests;

[TestFixture]
public class YoutubeServiceTests
{
    private readonly string _validVideoId = "dQw4w9WgXcQ";
    private readonly string _invalidVideoId = "invalid_id";
    private readonly Mock<ILogger<YoutubeService>> _loggerMock = new();
    private readonly Mock<IOptions<AppConfig>> _configMock = new();
    private readonly YoutubeService _youtubeService;

    public YoutubeServiceTests()
    {
        _configMock.SetupGet(x => x.Value).Returns(new AppConfig
        {
            YtDlPath = "yt-dlp"
        });

        _youtubeService = new YoutubeService(_loggerMock.Object, _configMock.Object);
    }


    [Test]
    public async Task GetVideoInfo_ValidId_ReturnsVideoInfo()
    {
        // Arrange

        // Act
        var result = await _youtubeService.GetVideoInfo(_validVideoId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(_validVideoId));
        Assert.IsNotNull(result.Title);
        Assert.IsNotNull(result.Duration);
        Assert.IsNotNull(result.Description);
    }

    [Test]
    public async Task GetVideoInfo_InvalidId_ReturnsNull()
    {
        // Arrange

        // Act
        var result = await _youtubeService.GetVideoInfo(_invalidVideoId);

        // Assert
        Assert.IsNull(result);
    }
}
