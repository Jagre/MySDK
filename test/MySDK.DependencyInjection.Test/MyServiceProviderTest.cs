using Microsoft.Extensions.DependencyInjection;
using MySDK.DependencyInjection.Test.A;
using MySDK.DependencyInjection.Test.B1;
using MySDK.DependencyInjection.Test.B2;
using MySDK.DependencyInjection.Test.C;
using MySDK.DependencyInjection.Test.Models;
using System;
using Xunit;

namespace MySDK.DependencyInjection.Test
{
    public class MyServiceProviderTest
    {
        public MyServiceProviderTest()
        {
            var services = new ServiceCollection();
            services.RegisterTypes<IAModel>("Model");

            MyServiceProvider.Provider = services.BuildServiceProvider();
        }

        [Fact]
        public void Test1()
        {
            var obj1 = MyServiceProvider.GetService<IAModel>();
            Assert.IsType<AModel>(obj1);

            var obj2 = MyServiceProvider.GetService<IBModel>();
            Assert.IsType<BModel>(obj2);

            var obj3 = MyServiceProvider.GetService<ICModel>();
            Assert.NotNull(obj3);

            var obj4 = MyServiceProvider.GetService<CModel>();
            Assert.Null(obj4);

            var obj5 = MyServiceProvider.GetService<DModel>();
            Assert.Null(obj5);

            var obj6 = MyServiceProvider.GetService<IEModel>();
            Assert.Null(obj6);

            try
            {
                var obj7 = MyServiceProvider.GetService<IFModel>();
            }
            catch
            {
                //Assert.IsType<NullReferenceException>(ex.GetBaseException());
            }
        }
    }
}
