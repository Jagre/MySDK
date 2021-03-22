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

        public static string GetSiteUrl(this IConfiguration config, string siteName)
        {
            return GetSection(config, "SiteUrls")[siteName];
        }

        public static string GetConnectionString(this IConfiguration config, string connectionName)
        {
            return GetSection(config, "ConnectionStrings")[connectionName];
        }

        public static string GetAppSetting(this IConfiguration config, string settingName)
        {
            return GetSection(config, "AppSettings")[settingName];
        }
    }
}
