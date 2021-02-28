using Microsoft.Extensions.Logging;
using System;

namespace MySDK.Logger
{
    /// <summary>
    /// My Log entity
    /// </summary>
    public class MyLog
    {
        /// <summary>
        /// log level
        /// </summary>
        public LogLevel LogLevel { get; set; }

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
        /// message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// create date
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
