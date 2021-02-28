using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace MySDK.Logger
{
    public class MyLogger : ILogger, IDisposable
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private MyLoggerSender<MyLog> _sender;
        public MyLogger(IConfiguration configuration)
        {
            _loggerConfiguration = configuration.GetLoggerConfiguration();
            _sender = new MyLoggerSender<MyLog>(_loggerConfiguration);
        }

        class EmptyDisposable : IDisposable
        {
            public void Dispose()
            {
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new EmptyDisposable();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = string.Empty;
            if (formatter != null)
                message = formatter.Invoke(state, exception);
            else
                message = $"{state} \r\nErrorMessage: {exception.Message}\r\nStackTrace: {exception.StackTrace}";

            if (!IsEnabled(logLevel))
            {
                Console.WriteLine($"[{LogLevel.Trace.ToString()} {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}]: {message}");
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                var log = new MyLog
                {
                    LogLevel = logLevel,
                    ApplicationName = _loggerConfiguration.ApplicationName,
                    MachineIP = _loggerConfiguration.MachineIP,
                    SystemName = _loggerConfiguration.SystemName,
                    Message = message,
                    CreateDate = DateTime.Now
                };
                _sender.Sending(log);
            }
        }

        public void Dispose()
        {
            if (_sender != null)
            {
                _sender.Dispose();
                _sender = null;
            }
        }
    }
}
