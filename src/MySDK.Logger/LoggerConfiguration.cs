using Microsoft.Extensions.Configuration;
using MySDK.Configuration;
using System;

namespace MySDK.Logger
{
    public class LoggerConfiguration
    {
        /// <summary>
        /// the log url that is API of saving all logs
        /// </summary>
        public string LogURL { get; set; }

        /// <summary>
        /// the machine ip that ocurre error
        /// </summary>
        public string MachineIP { get; set; }

        /// <summary>
        /// system name
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// application name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// logger's max queue length
        /// </summary>
        public int MaxQueueLength { get; set; }

        /// <summary>
        /// logging intervel (with unit millisecond)
        /// </summary>
        public long SaveLogIntervel { get; set; }

        /// <summary>
        ///  logging count every time
        /// </summary>
        public long SaveLogCountEveryTime { get; set; }

    }


    public static class RedisConifgurationExtension
    {
        private static LoggerConfiguration _loggerConfiguration;

        public static LoggerConfiguration GetLoggerConfiguration(this IConfiguration root)
        {
            if (_loggerConfiguration != null)
                return _loggerConfiguration;

            _loggerConfiguration = root.GetConfiguration<LoggerConfiguration>("LoggerConfiguration");
            if (_loggerConfiguration == null)
            {
                throw new NullReferenceException("Logger configuration object is null.");
            }

            if (_loggerConfiguration.MaxQueueLength == 0)
                _loggerConfiguration.MaxQueueLength = 200;

            if (_loggerConfiguration.SaveLogCountEveryTime == 0)
                _loggerConfiguration.SaveLogCountEveryTime = 20;

            if (_loggerConfiguration.SaveLogIntervel == 0)
                _loggerConfiguration.SaveLogIntervel = 2000;

            return _loggerConfiguration;
        }
    }
}
