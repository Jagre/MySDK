using MySDK.Serialization;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace MySDK.Redis
{
    public class RedisRepository : RedisContext, IRedisRepository
    {
        public RedisRepository(string redisServerName)
            : base(redisServerName)
        {
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            return await DB.KeyExistsAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var data = await DB.StringGetAsync(key);
            return data.ToString().FromJson<T>();
        }

        public Task<bool> LockAsync(string key, string value, TimeSpan expiry)
        {
            return DB.LockTakeAsync(key, value, expiry, CommandFlags.None);
        }

        public async Task<bool> RemoveAsync(string key)
        {
            return await DB.KeyDeleteAsync(key);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            //when expiry is null that imply permanent
            return await DB.StringSetAsync(key, value.ToJson(), expiry, When.Always);
        }

        public async Task<bool> UnlockAsync(string key, string value)
        {
            return await DB.LockReleaseAsync(key, value);
        }
    }
}