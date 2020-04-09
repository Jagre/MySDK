using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace MySDK.Redis
{
    public abstract class RedisContext
    {

        protected IDatabase DB { get; private set; }


        public RedisContext(IConfigurationRoot root, string redisServerName)
            : this(root.GetRedisConfiguration(redisServerName))
        {
        }

        public RedisContext(RedisConfiguration config)
        {
            IConnectionMultiplexer conn = ConnectionMultiplexer.Connect(config.ToString());
            DB = conn.GetDatabase();
        }

    }
}