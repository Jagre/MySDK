using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MySDK.Logger
{
    public class MyLoggerProvider : ILoggerProvider
    {
        private ILogger _logger;

        public MyLoggerProvider(IConfiguration configuration)
        {
            _logger = new MyLogger(configuration);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _logger;
        }

        public void Dispose()
        {
            if (_logger != null)
            {
                _logger = null;
            }
        }
    }
}
