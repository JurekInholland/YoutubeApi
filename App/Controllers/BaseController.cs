using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>
/// Base for all controllers
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public abstract class BaseController : ControllerBase
{
}
