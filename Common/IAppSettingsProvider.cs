namespace Common
{
    public interface IAppSettingsProvider
    {
        Task<Dictionary<string, string>> GetAllSettingsAsync();
    }
}
