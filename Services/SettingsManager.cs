using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Models;
using Models.Requests;

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

    public async Task SetSettings(UpdateSettingsRequest settingsRequest)
    {
        var settings = await GetSettings();

        if (settingsRequest.NamingFormat != null)
            settings.NamingFormat = settingsRequest.NamingFormat;
        if (settingsRequest.DownloadPath != null)
            settings.DownloadPath = settingsRequest.DownloadPath;
        if (settingsRequest.WorkInterval != null)
            settings.WorkInterval = settingsRequest.WorkInterval.Value;
        if (settingsRequest.CleanUpInterval != null)
            settings.CleanUpInterval = settingsRequest.CleanUpInterval.Value;

        if (settingsRequest.MaxVideoDuration != null)
            settings.MaxVideoDuration = TimeSpan.FromSeconds(settingsRequest.MaxVideoDuration.Value);

        await _unitOfWork.ApplicationSettings.SetSettings(settings);
        await _unitOfWork.Save();
    }
}
