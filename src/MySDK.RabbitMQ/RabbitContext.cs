using Microsoft.Extensions.Configuration;
using MySDK.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace MySDK.RabbitMQ
{
    public class RabbitContext: IDisposable
    {
        private IConnection _connection;

        public IConnection Connection
        {
            get
            {
                if (_connection == null)
                    throw new NullReferenceException("The connecetion object of the rabbit MQ hasn't initialized.");
                return _connection;
            }
            protected set
            {
                _connection = value;
            }
        }

        public RabbitContext(string connectionName)
        {
            var connectionString = MyServiceProvider.Configuration.GetConnectionString(connectionName);
            var factory = new ConnectionFactory()
            {
                Uri = new Uri(connectionString),
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                SocketReadTimeout = TimeSpan.FromMilliseconds(1500),
                SocketWriteTimeout = TimeSpan.FromMilliseconds(1500),
                ContinuationTimeout = TimeSpan.FromMilliseconds(1500),
                RequestedHeartbeat = TimeSpan.FromMilliseconds(3000),
                NetworkRecoveryInterval = TimeSpan.FromMilliseconds(3000),
                RequestedConnectionTimeout = TimeSpan.FromMilliseconds(3000)
            };

            _connection = factory.CreateConnection();
        }

        public virtual void Dispose()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
