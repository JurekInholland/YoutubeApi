using Models;

namespace Services;

public interface ISettingsManager
{
    public Task<ApplicationSettings> GetSettings();
    public Task SetSettings(ApplicationSettings settings);
}
