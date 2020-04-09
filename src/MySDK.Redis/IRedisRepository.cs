using System;
using System.Threading.Tasks;

namespace MySDK.Redis
{
    /// <summary>
    /// Just simple functions (no hashset, no sub/pub, no transaction, no stream ...)
    /// </summary>
    public interface IRedisRepository
    {
        Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry);

        Task<bool> RemoveAsync(string key);

        Task<T> GetAsync<T>(string key);

        Task<bool> ContainsKeyAsync(string key);

        Task<bool> LockAsync(string key, string value, TimeSpan expiry);

        Task<bool> UnlockAsync(string key, string value);
    }
}