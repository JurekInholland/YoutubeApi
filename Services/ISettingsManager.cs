using Models;
using Models.Requests;

namespace Services;

public interface ISettingsManager
{
    public Task<ApplicationSettings> GetSettings();
    public Task SetSettings(UpdateSettingsRequest settingsRequest);
}
