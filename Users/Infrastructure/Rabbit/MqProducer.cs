using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Rabbit
{
    public class MqProducer : MQBase
    {
        public MqProducer(IChannel channel) : base(channel)
        {
        }
        public async Task PublishMessageAsync(string queueName, string message)
        {
            await CreateQueueIfNotExists(queueName);
            await _channel.BasicPublishAsync(exchange: "",
                                     routingKey: queueName,);
        }
    }
}
