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

        /// <summary>
        /// Inner static class for Application-specific settings
        /// </summary>
        public static class App
        {
            // Static properties to retrieve settings from cache or configuration
            public static string AdminPassword => GetSetting("App:AdminPassword");
            public static string Chrome => GetSetting("App:Chrome");
            public static string CroppedImageFileName => GetSetting("App:CroppedImageFileName");
            public static string CroppedImagePath => GetSetting("App:CroppedImagePath");
            public static string CroppedMainImageFileName => GetSetting("App:CroppedMainImageFileName");
            public static string ImageSelectFilter => GetSetting("App:ImageSelectFilter");
            public static string PhoneNumber => GetSetting("App:PhoneNumber");
            public static string RedirectVoiceMessageNumber => GetSetting("App:RedirectVoiceMessageNumber");
            public static string TurboAzUrl => GetSetting("App:TurboAzUrl");
            public static string UrlStartsWith => GetSetting("App:UrlStartsWith");
            public static string WhatsApp => GetSetting("App:WhatsApp");
            public static string WhatsappMessageUrl => GetSetting("App:WhatsappMessageUrl");
        }

        /// <summary>
        /// Inner static class for Connection Strings
        /// </summary>
        public static class ConnectionStrings
        {
            public static string DefaultConnection => _configuration.GetConnectionString("DefaultConnection");
        }
    }
}
