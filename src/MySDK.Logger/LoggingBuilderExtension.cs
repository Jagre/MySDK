using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MySDK.Logger
{
    public static class LoggingBuilderExtension
    {
        public static ILoggingBuilder AddCustomLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            return builder.AddProvider(new MyLoggerProvider(configuration));
        }
    }
}
