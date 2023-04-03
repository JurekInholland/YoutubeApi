using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

/// <summary>
///   Settings controller
/// </summary>
public class SettingsController : BaseController
{
    /// <summary>
    ///    Update the settings
    /// </summary>
    [HttpPut("", Name = nameof(UpdateSettings))]
    public IActionResult UpdateSettings()
    {
        return Ok();
    }
}
