using Dapper.Contrib.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MySDK.Dapper.Extensions
{
    public static class EntityTableExtension
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _keys = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> _explicityKeys = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        public static string GetPrimaryKeyName(this Type type)
        {
            //var properties = type.GetProperties(System.Reflection.BindingFlags.Public);
            //if (null != properties || !properties.Any())
            //    return string.Empty;

            //var property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<KeyAttribute>().Any());

            //if (null == property)
            //{
            //    property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<ExplicitKeyAttribute>().Any());
            //}
            var properties = type.GetPrimaryKeyProperties();
            if (properties.Any())
            {
                var property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<KeyAttribute>().Any());
                if (null == property)
                {
                    property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<ExplicitKeyAttribute>().Any());
                }
                return property?.Name;
            }
            return string.Empty;
        }

        public static IEnumerable<PropertyInfo> GetPrimaryKeyProperties(this Type type)
        {
            if (!_keys.ContainsKey(type.TypeHandle))
            {
                var properties = type.GetProperties().Where(i => i.GetCustomAttributes(true).OfType<KeyAttribute>().Any());
                if (properties != null && properties.Any())
                {
                    _keys.TryAdd(type.TypeHandle, properties);
                }
            }
            if (!_explicityKeys.ContainsKey(type.TypeHandle))
            {
                var properties = type.GetProperties()?.Where(i => i.GetCustomAttributes(true).OfType<ExplicitKeyAttribute>().Any());
                if (properties != null && properties.Any())
                {
                    _explicityKeys.TryAdd(type.TypeHandle, properties);
                }
            }

            if (_keys.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> properties1))
            {
                return properties1;
            }
            else if (_explicityKeys.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> properties2))
            {
                return properties2;
            }
            return new List<PropertyInfo>();
        }
    }
}
