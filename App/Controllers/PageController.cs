using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>
/// Controller for web pages
/// </summary>
[ApiController]
public class PageController : ControllerBase
{
    private static readonly string IndexHtml = System.IO.File.ReadAllText("wwwroot/index.html");

    /// <summary>
    /// Get the index page
    /// </summary>
    [HttpGet("/{route?}", Name = nameof(Index))]
    public IActionResult Index([FromRoute] string? route = "")
    {
        return new ContentResult
        {
            Content = IndexHtml,

            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
