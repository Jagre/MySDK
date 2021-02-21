using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySDK.DependencyInjection;
using MySDK.Redis;
using System.IO;

namespace MySKD.Redis.Test
{
    public class TestBase
    {
        public TestBase()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("redisConfiguration.json", optional: true, reloadOnChange: true)
                .Build();
            MyServiceProvider.Configuration = configRoot;

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IRedisRepository>(new RedisRepository("myRedisServerName"));
            MyServiceProvider.Provider = services.BuildServiceProvider();
        }

        
    }
}
