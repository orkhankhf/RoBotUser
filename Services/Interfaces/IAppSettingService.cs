using Entities.Models;

namespace Services.Interfaces
{
    public interface IAppSettingService : IGenericService<AppSetting>
    {
        Task<AppSetting> GetSettingByKeyAsync(string key);
        Task<List<AppSetting>> GetAllSettingsAsync();
    }
}
