using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MySDK.DependencyInjection
{
    public class MyServiceProvider
    {
        public static IServiceProvider Provider { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static T GetService<T>()
        {
            if (Provider == null)
            {
                throw new NullReferenceException("Haven't setted ServiceProvider object");
            }
            return Provider.GetService<T>();
        }
    }
}
