using Microsoft.Extensions.Logging;
using MySDK.DependencyInjection;
using MySDK.Serialization;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MySDK.RabbitMQ
{
    public class ProducerBase : RabbitContext, IDisposable
    {
        private IModel _channel;

        protected string QueueName;
        protected string RoutingKey;
        protected string ExchangeName;
        protected string ExchangeType;
        protected bool Durable;

        private readonly ILogger _logger;

        public ProducerBase(string connectionName, string queueName, string routingKey, string exchangeName, string exchangeType = "direct", bool durable = true)
            : base(connectionName)
        {
            QueueName = queueName;
            RoutingKey = routingKey;
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
            Durable = durable;

            _logger = MyServiceProvider.GetService<ILogger>();

            _channel = Connection.CreateModel();
        }

        public virtual bool PublishingAsync<T>(List<T> messageObjects)
        {
            _channel.ExchangeDeclare(ExchangeName, ExchangeType, Durable, false, null);
            _channel.QueueDeclare(QueueName, Durable, false, false, null);
            _channel.QueueBind(QueueName, ExchangeName, RoutingKey);

            var messages = new Queue<string>();
            try
            {
                foreach (var obj in messageObjects)
                {
                    messages.Enqueue(obj.ToJson());
                }
                while (messages.Any())
                {
                    var body = Encoding.UTF8.GetBytes(messages.Dequeue());
                    _channel.BasicPublish(
                        exchange: ExchangeName,
                        routingKey: RoutingKey,
                        body: body);
                }
                return true;
            }
            catch (Exception ex)
            {
                while (messages.Any())
                {
                    Logging(ex, messages.Dequeue());
                }

                return false;
            }
        }

        protected virtual void Logging(Exception exception, string messageBody)
        {
            _logger.LogError(exception, messageBody);
        }

        public override void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel.Dispose();
                _channel = null;
            }
            base.Dispose();
        }
    }
}
