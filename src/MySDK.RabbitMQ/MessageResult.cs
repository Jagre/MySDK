using RabbitMQ.Client;
using System;

namespace MySDK.RabbitMQ
{
    /// <summary>
    /// 消息执行结果
    /// </summary>
    public class MessageResult
    {
        private readonly ulong _deliveryTag;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="deliveryTag"></param>
        /// <param name="queueName"></param>
        /// <param name="consumerName"></param>
        /// <param name="messageBody"></param>
        public MessageResult(IModel channel, ulong deliveryTag, string queueName, string consumerName, string messageBody)
        {
            _deliveryTag = deliveryTag;
            Channel = channel;
            QueueName = queueName;
            ConsumerName = consumerName;
            MessageBody = messageBody;
        }

        /// <summary>
        /// Consumer channel object
        /// </summary>
        public IModel Channel { get; }

        /// <summary>
        /// Queue name
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Consumer name
        /// </summary>
        public string ConsumerName { get; set; }

        /// <summary>
        /// Message body (json)
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// Error content
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Is successful
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Consumer excuting begin time
        /// </summary>
        public DateTime BeginTime => DateTime.Now;

        /// <summary>
        /// Consumer excuting end time
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Consumer expense time
        /// </summary>
        public TimeSpan ExpenseTime => EndTime - BeginTime;

        /// <summary>
        /// 消息是否得到应答
        /// </summary>
        public bool IsAcked { get; protected set; }

        /// <summary>
        /// 消息确认
        /// </summary>
        public void Confirm()
        {
            if (!IsAcked && Channel.IsOpen)
            {
                Channel.BasicAck(_deliveryTag, false);
                IsAcked = true;
            }
        }

        ///// <summary>
        ///// 消息拒绝
        ///// </summary>
        //public void Reject()
        //{
        //    if (!IsAcked && Channel.IsOpen)
        //    {
        //        Channel.BasicAck(_deliveryTag, false);
        //        IsAcked = true;
        //    }
        //}
    }
}
