using Application.Services;
using Entities.Contracts.Services;
using Entities.Exceptions;
using Entities.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Entities.Contracts;
using System.Threading.Tasks;

namespace Infrastructure.RabbitMq
{
    public class RpcServer : IRpcServer
    {
        private IChannel _channel;
        private const string QUEUE_NAME = "transport_rpc";
        private readonly ITransportManager _transportManager;
        public RpcServer(ITransportManager transportManager) 
        {
            _transportManager = transportManager;
        }

        public async Task StartAsync(IChannel channel)
        {
            _channel = channel;

            await _channel.QueueDeclareAsync(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task CreateConsumer()
        {
            await _channel.QueueDeclareAsync(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var replyToQueue = ea.BasicProperties.ReplyTo;

                if (replyToQueue is null)
                    throw new QueueNotFoundException("Your replyQueue name is null.");

                var replyProps = new BasicProperties
                {
                    CorrelationId = ea.BasicProperties.CorrelationId
                };

                var body = ea.Body.ToArray();

                if (body.Length == 0)
                    throw new UserIdNotFoundException("User id is null.");

                IEnumerable<Transport> answer = null;

                try
                {
                    var uid = Encoding.UTF8.GetString(body);

                    answer = await _transportManager.GetUsersTransport(uid);
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [.] {e.Message}");
                }
                finally
                {
                    var answerJson = JsonSerializer.Serialize(answer);
                    var answerBytes = Encoding.UTF8.GetBytes(answerJson);

                    await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: replyToQueue, body: answerBytes, basicProperties: replyProps, mandatory: true);
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            await _channel.BasicConsumeAsync(queue: QUEUE_NAME, autoAck: false, consumer: consumer);
        }
    }
}
