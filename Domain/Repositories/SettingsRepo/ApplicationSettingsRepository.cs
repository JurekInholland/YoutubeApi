using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Domain.Repositories.SettingsRepo;

public class ApplicationSettingsRepository : RepositoryBase<ApplicationSettings>, IApplicationSettingsRepository
{
    private YoutubeAppContext YoutubeContext => (YoutubeAppContext) Context;

    public ApplicationSettingsRepository(DbContext context) : base(context)
    {
        Context = context;
    }

    public async Task<ApplicationSettings> GetSettings()
    {
        return await YoutubeContext.ApplicationSettings.OrderBy(x => x.Id).FirstOrDefaultAsync() ?? new ApplicationSettings();
    }

    public async Task SetSettings(ApplicationSettings settings)
    {
        var existing = await GetSettings();
        Context.Entry(existing).CurrentValues.SetValues(settings);
    }
}
