using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace MySDK.DependencyInjection
{
    public static class MyServiceProviderExtension
    {
        /// <summary>
        /// According to suffix name to register all matched types (interfaces, classes), 
        /// but excludes abstract, sealed, static and nested
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="suffix"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterTypes<T>(this IServiceCollection services, string suffix, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
            var t = typeof(T);
            var types = t.Assembly.GetTypes().Where(i => i.Name.EndsWith(suffix, StringComparison.CurrentCulture));

            foreach (var type in types)
            {
                if (!type.IsAbstract && !type.IsSealed && !type.IsNested && type.IsClass)
                {
                    var typeInterface = type.GetInterfaces()?.FirstOrDefault(i => i.IsInterface && i.Name.EndsWith(suffix, StringComparison.CurrentCulture));
                    if (typeInterface == null)
                    {
                        services.Add(ServiceDescriptor.Describe(type, type, lifetime));
                    }
                    else
                    {
                        services.Add(ServiceDescriptor.Describe(typeInterface, type, lifetime));
                    }
                }
            }

            return services;
        }
    }
}
