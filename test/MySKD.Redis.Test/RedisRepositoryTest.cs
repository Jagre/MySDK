using MySDK.DependencyInjection;
using MySDK.Redis;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MySKD.Redis.Test
{
    public class RedisRepositoryTest: TestBase
    {
        public RedisRepositoryTest()
        {
        }

        [Fact]
        public async Task Test_SetAsync_Ok()
        {
            var repo = MyServiceProvider.GetService<IRedisRepository>();
            var flag = await repo.SetAsync("test", new byte[] { 97, 98, 99 }, TimeSpan.FromSeconds(100));
            Assert.True(flag);
        }
    }
}
