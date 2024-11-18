using Data.Interfaces;
using Entities.Models;
using Services.Interfaces;

namespace Services.Implementations
{
    public class AppSettingService : GenericService<AppSetting>, IAppSettingService
    {
        private readonly IAppSettingRepository _appSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AppSettingService(IAppSettingRepository appSettingsRepository, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _appSettingsRepository = appSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<AppSetting> GetSettingByKeyAsync(string key)
        {
            return await _appSettingsRepository.SingleOrDefaultAsync(s => s.Key == key);
        }

        public async Task<List<AppSetting>> GetAllSettingsAsync()
        {
            return (await _appSettingsRepository.GetAllAsync()).ToList();
        }
    }
}
