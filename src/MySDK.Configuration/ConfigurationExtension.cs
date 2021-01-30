using Microsoft.Extensions.Configuration;

namespace MySDK.Configuration
{
    public static class ConfigurationExtension
    {
        private static IConfigurationSection GetSection(IConfigurationRoot config, string sectionName)
        {
            return config.GetSection(sectionName);
        }

        public static T GetConfiguration<T>(this IConfigurationRoot config, string sectionName) where T : class, new()
        {
            var bindObj = new T();
            GetSection(config, sectionName).Bind(bindObj);
            return bindObj;
        }

        public static string GetSiteConfig(this IConfigurationRoot config, string siteName)
        {
            return GetSection(config, "Sites")[siteName];
        }

        public static string GetConnectionConfig(this IConfigurationRoot config, string connectionName)
        {
            return GetSection(config, "Connections")[connectionName];
        }

        public static string GetAppSettingConfig(this IConfigurationRoot config, string settingName)
        {
            return GetSection(config, "AppSettings")[settingName];
        }
    }
}
