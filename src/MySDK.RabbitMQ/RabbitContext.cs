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
                NetworkRecoveryInterval = TimeSpan.FromSeconds(20),
                //AutomaticRecoveryEnabled = true,
                //TopologyRecoveryEnabled = true
            };

            _connection = factory.CreateConnection();
        }

        public virtual void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
