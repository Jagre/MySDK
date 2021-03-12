using Microsoft.Extensions.Logging;
using MySDK.DependencyInjection;
using System;
using Xunit;

namespace MySDK.Logger.Test
{
    public class LoggerTest: TestBase
    {
        [Fact]
        public void Test_Logging_Ok()
        {
            var logger = MyServiceProvider.GetService<ILogger>();

            var ex = new NullReferenceException("the test object is null");
            logger.LogError(ex, "run to hehe ocurred error");
        }
    }
}
