using Microsoft.AspNetCore.Mvc;
using Services.YoutubeService;

namespace App.Controllers;

/// <summary>
/// YoutubeController
/// </summary>
public class YoutubeController : BaseController
{
    private readonly ILogger<YoutubeController> _logger;
    private readonly IYoutubeService _youtubeService;


    /// <summary>
    /// YoutubeController constructor
    /// </summary>
    public YoutubeController(ILogger<YoutubeController> logger, IYoutubeService youtubeService)
    {
        _logger = logger;
        _youtubeService = youtubeService;
    }


    /// <summary>
    /// Get search completion from google api
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet(nameof(SearchCompletion))]
    public async Task<IActionResult> SearchCompletion(string query)
    {
        string?[] result = await _youtubeService.GetSearchCompletion(query);
        return Ok(result);
    }
}
