using Microsoft.Extensions.Configuration;

namespace MySDK.Configuration
{
    public static class ConfigurationExtension
    {
        private static IConfigurationSection GetSection(IConfiguration config, string sectionName)
        {
            return config.GetSection(sectionName);
        }

        public static T GetConfiguration<T>(this IConfiguration config, string sectionName) where T : class, new()
        {
            var bindObj = new T();
            GetSection(config, sectionName).Bind(bindObj);
            return bindObj;
        }

        public static string GetSiteConfig(this IConfiguration config, string siteName)
        {
            return GetSection(config, "Sites")[siteName];
        }

        public static string GetConnectionConfig(this IConfiguration config, string connectionName)
        {
            return GetSection(config, "Connections")[connectionName];
        }

        public static string GetAppSettingConfig(this IConfiguration config, string settingName)
        {
            return GetSection(config, "AppSettings")[settingName];
        }
    }
}
