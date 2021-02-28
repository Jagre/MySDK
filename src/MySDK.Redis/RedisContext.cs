using MySDK.DependencyInjection;
using StackExchange.Redis;
using System;

namespace MySDK.Redis
{
    public class RedisContext
    {
        private readonly IDatabase _db;
        public IDatabase DB
        {
            get
            {
                if (_db == null)
                    throw new NullReferenceException("Redis connection object hasn't been initialzed.");
                return _db;
            }
        }

        public RedisContext(string redisServerName)
        {
            var redisConfiguration = MyServiceProvider.Configuration.GetRedisConfiguration(redisServerName);
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration.ToString());
            _db = connectionMultiplexer?.GetDatabase();
        }
    }
}