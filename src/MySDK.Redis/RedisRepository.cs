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
            if (!data.HasValue)
            {
                return default(T);
            }

            var type = typeof(T);
            if (IsBasicType(type))
            {
                return (T)ConvertToBasicTypeObject<T>(data);
            }
            else
            {
                return data.ToString().FromJson<T>();
            }
        }

        private object ConvertToBasicTypeObject<T>(RedisValue data)
        {
            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    int temp1;
                    if (data.TryParse(out temp1))
                    {
                        return temp1;
                    }
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    int temp2;
                    if (data.TryParse(out temp2))
                    {
                        return temp2;
                    }
                    break;
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Single:
                    double temp3;
                    if (data.TryParse(out temp3))
                    {
                        return temp3;
                    }
                    break;
                case TypeCode.String:
                    return data.ToString();
            }
            return default(T);
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
            string redisValue = string.Empty;
            if (IsBasicType(typeof(T)))
            {
                redisValue = RedisValue.Unbox(value);
            }
            else
            {
                redisValue = value.ToJson();
            }

            //when expiry is null that imply permanent
            return await DB.StringSetAsync(key, redisValue, expiry, When.Always);
        }

        private bool IsBasicType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                case TypeCode.String:
                    return true;
            }
            return false;
        }

        public async Task<bool> UnlockAsync(string key, string value)
        {
            return await DB.LockReleaseAsync(key, value);
        }
    }
}