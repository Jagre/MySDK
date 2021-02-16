using Dapper.Contrib.Extensions;
using System;
using System.Linq;

namespace MySDK.Dapper.Extentions
{
    public static class EntityExtention
    {
        public static string GetPrimaryKeyName(this Type type)
        {
            var properties = type.GetProperties(System.Reflection.BindingFlags.Public);
            if (null != properties || !properties.Any())
                return string.Empty;

            var property = properties.FirstOrDefault(i => i.Name.ToLower() == "id");
            if (property == null)
            {
                property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<KeyAttribute>().Any());
                if (null == property)
                {
                    property = properties.FirstOrDefault(i => i.GetCustomAttributes(true).OfType<ExplicitKeyAttribute>().Any());
                }
            }
            return property?.Name;
        }
    }
}
