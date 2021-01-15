using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MySDK.DependencyInjection
{
    public class MyServiceProvider
    {
        public static IServiceProvider Provider { get; set; }

        public static IConfiguration Configuration { get; set; }

        public static T GetInstance<T>()
        {
            if (Provider == null)
            {
                throw new Exception("Havn't setted ServiceProvider object");
            }
            return Provider.GetService<T>();
        }
    }
}
