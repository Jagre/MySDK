using MySDK.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

namespace MySDK.RabbitMQ
{
    /// <summary>
    /// Consumer base class (T is type of the message body object )
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConsumerBase<T> : RabbitContext, IDisposable
    {
        private IModel _channel;

        protected string QueueName;
        protected string RoutingKey;
        protected string ExchangeName;
        protected string ExchangeType;

        public Action<MessageResult> BeforeMessageHandling;
        public Action<MessageResult> AfterMessageHandled;

        public ConsumerBase(string connectionName, string queueName, string routingKey, string exchangeName, string exchangeType = "direct", bool durable = true) :
            base(connectionName)
        {
            QueueName = queueName;
            RoutingKey = routingKey;
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;

            _channel = Connection.CreateModel();
            
            // exchange
            _channel.ExchangeDeclare(exchange: exchangeName,
                type: exchangeType,
                durable: durable,
                autoDelete: false,
                arguments: null);

            // queue
            _channel.QueueDeclare(queue: queueName,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // binding
            _channel.QueueBind(queueName, exchangeName, routingKey, null);

            _channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnReceived;
        }

        protected async void OnReceived(object? sender, BasicDeliverEventArgs args)
        {
            var messageBody = Encoding.UTF8.GetString(args.Body.ToArray());
            var param = messageBody.FromJson<T>();
            var msgResult = new MessageResult(_channel, args.DeliveryTag, QueueName, GetType().Name, messageBody);
            BeforeMessageHandling?.Invoke(msgResult);
            try
            {
                msgResult.IsSuccessful = await MessageHandlingAsync(param, msgResult);
            }
            catch (Exception ex)
            {
                msgResult.Error = $"ErrorMessage: {ex.Message}; \r\nStatckTrace: {ex.StackTrace}";
            }
            msgResult.Confirm();

            AfterMessageHandled?.Invoke(msgResult);
        }

        /// <summary>
        /// Handling message
        /// </summary>
        /// <param name="messageBody">Message body</param>
        /// <param name="messageResult">Message result</param>
        /// <returns>Success flag</returns>
        public abstract Task<bool> MessageHandlingAsync(T messageBody, MessageResult messageResult);

        public override void Dispose()
        {
            if (_channel != null)
            {
                _channel.Dispose();
            }
            base.Dispose();
        }
    }
}
