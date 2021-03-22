using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySDK.DependencyInjection;
using System.IO;

namespace MySDK.Dapper.Test
{
    public class TestBase
    {
        protected IConfigurationRoot ConfigurationRoot;
        
        public TestBase()
        {
            var basePath = Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder();
            ConfigurationRoot = builder.SetBasePath(basePath)
                .AddJsonFile("configuration.json", true, true)
                .Build();

            //IServiceCollection services = new ServiceCollection();

            MyServiceProvider.Configuration = ConfigurationRoot;
            //MyServiceProvider.Provider = services.BuildServiceProvider();
        }
    }
}
