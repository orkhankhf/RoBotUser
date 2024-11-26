using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Common
{
    public static class AppSettings
    {
        private static readonly IConfigurationRoot _configuration;
        private static readonly ConcurrentDictionary<string, string> _databaseSettingsCache = new();

        static AppSettings()
        {
            // Load configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Get a setting value from the cache or appsettings.json.
        /// </summary>
        public static string GetSetting(string key)
        {
            if (_databaseSettingsCache.TryGetValue(key, out string value))
            {
                return value;
            }

            // Fall back to appsettings.json if the key is not found in the database cache
            return _configuration[key] ?? throw new KeyNotFoundException($"Setting with key '{key}' was not found.");
        }

        /// <summary>
        /// Inner static class for API settings
        /// </summary>
        public static class Api
        {
            public static string BaseUrl => GetSetting("Api:BaseUrl");
        }
    }
}
