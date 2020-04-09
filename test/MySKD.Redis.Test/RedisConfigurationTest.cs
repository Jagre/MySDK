using Microsoft.Extensions.Configuration;
using MySDK.Redis;
using System.IO;
using Xunit;

namespace MySKD.Redis.Test
{
    public class RedisConfigurationTest
    {
        [Fact]
        public void GetRedisConfigurationsTest()
        {
            var basePath = Directory.GetCurrentDirectory();
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("redisConfiguration.json", optional: true, reloadOnChange: true)
                .Build();

            var configurations = configRoot.GetRedisConfigurations();
            var count = configurations.Count;
            Assert.Equal(1, count);
        }
    }
}
