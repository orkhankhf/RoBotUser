using Common;
using Services.Interfaces;

namespace Services.Implementations
{
    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IAppSettingService _appSettingService;

        public AppSettingsProvider(IAppSettingService appSettingService)
        {
            _appSettingService = appSettingService;
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            var settings = await _appSettingService.GetAllSettingsAsync();
            return settings.ToDictionary(s => s.Key, s => s.Value);
        }
    }
}
