using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Models.Requests;
using Services;

namespace App.Controllers;

/// <summary>
///   Settings controller
/// </summary>
public class SettingsController : BaseController
{
    private readonly ISettingsManager _settingsManager;

    /// <summary>
    ///  SettingsController constructor
    /// </summary>
    public SettingsController(ISettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
    }

    /// <summary>
    ///    Update the settings
    /// </summary>
    [HttpPut("", Name = nameof(UpdateSettings))]
    public async Task<IActionResult> UpdateSettings(
        [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)]
        UpdateSettingsRequest settingsRequest)
    {
        try
        {
            await _settingsManager.SetSettings(settingsRequest);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}
