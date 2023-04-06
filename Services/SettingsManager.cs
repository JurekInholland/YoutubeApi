using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Models;

namespace Services;

public class SettingsManager : ISettingsManager
{
    private readonly ILogger<SettingsManager> _logger;
    private readonly IUnitOfWork _unitOfWork;

    private readonly ApplicationSettings _settings;

    public SettingsManager(ILogger<SettingsManager> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;

        _settings = _unitOfWork.ApplicationSettings.GetSettings().Result;
    }


    public Task<ApplicationSettings> GetSettings()
    {
        return _unitOfWork.ApplicationSettings.GetSettings();
    }

    public async Task SetSettings(ApplicationSettings settings)
    {
        _logger.LogInformation("Updating settings");
        settings.Id = _settings.Id;
        _unitOfWork.ApplicationSettings.Update(settings);
        await _unitOfWork.Save();
    }
}
