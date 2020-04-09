using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MySDK.Redis
{
    public class RedisRepository : RedisContext, IRedisRepository
    {
        public RedisRepository(RedisConfiguration configuration)
            : base(configuration)
        {
        }

        public RedisRepository(IConfigurationRoot root, string redisServerName)
            : base(root, redisServerName)
        {
        }

        public async Task<bool> ContainsKeyAsync(string key)
        {
            return await DB.KeyExistsAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var json = await DB.StringGetAsync(key);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
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
            var json = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            //when expiry is null that imply permanent
            return await DB.StringSetAsync(key, json, expiry, When.Always);
        }

        public async Task<bool> UnlockAsync(string key, string value)
        {
            return await DB.LockReleaseAsync(key, value);
        }
    }
}