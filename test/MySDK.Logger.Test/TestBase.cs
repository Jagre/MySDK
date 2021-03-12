using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySDK.DependencyInjection;
using System.IO;

namespace MySDK.Logger.Test
{
    public class TestBase
    {
        protected IConfigurationRoot ConfigurationRoot;

        public TestBase()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder();
            ConfigurationRoot = builder.SetBasePath(basePath)
                .AddJsonFile("logging.json", true, true)
                .Build();

            MyServiceProvider.Configuration = ConfigurationRoot;

            IServiceCollection services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.AddCustomLogging(ConfigurationRoot);
            });
            //services.AddSingleton<ILogger>((provider) => provider.GetService<ILoggerFactory>().CreateLogger<TestBase>());
            services.AddSingleton<ILogger>((provider) => provider.GetService<ILoggerProvider>().CreateLogger("ErrorLogger"));
            MyServiceProvider.Provider = services.BuildServiceProvider();
        }
    }
}
