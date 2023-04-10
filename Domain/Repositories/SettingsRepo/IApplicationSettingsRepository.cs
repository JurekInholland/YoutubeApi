using Models;

namespace Domain.Repositories.SettingsRepo;

public interface IApplicationSettingsRepository : IRepositoryBase<ApplicationSettings>
{
    public Task<ApplicationSettings> GetSettings();
    public Task SetSettings(ApplicationSettings settings);
}
