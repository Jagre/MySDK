using MySDK.DependencyInjection;
using MySDK.Redis;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MySKD.Redis.Test
{
    public class RedisRepositoryTest : TestBase
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
            var bytes = await repo.GetAsync<byte[]>("test");
            Assert.Equal((byte)98, bytes[1]);

            await repo.SetAsync("char", 'A', TimeSpan.FromSeconds(100));
            var char1 = await repo.GetAsync<char>("char");
            Assert.Equal('A', char1);

            await repo.SetAsync("int", 1000, TimeSpan.FromSeconds(100));
            var int1 = await repo.GetAsync<int>("int");
            Assert.Equal(1000, int1);
            Assert.IsType<int>(int1);

            await repo.SetAsync("string", "haha", TimeSpan.FromSeconds(100));
            var string1 = await repo.GetAsync<string>("string");
            Assert.Equal("haha", string1);

            await repo.SetAsync<dynamic>("object", new { orderId = 1, productId = 1 }, TimeSpan.FromSeconds(100));
            var object1 = await repo.GetAsync<dynamic>("object");
            Assert.Equal(1, (int)object1.orderId);
        }

    }
}
