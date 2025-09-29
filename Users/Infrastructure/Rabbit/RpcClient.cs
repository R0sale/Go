using Entities.Contracts;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Rabbit
{
    public class RpcClient : IRpcClient
    {
        private IChannel _channel;
        private string replyQueueName;
        private ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

        public RpcClient()
        {
        }

        public async Task StartAsync(IChannel channel)
        {
            _channel = channel;

            QueueDeclareOk queueResult = await _channel.QueueDeclareAsync();
            replyQueueName = queueResult.QueueName;

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                string? correlationId = ea.BasicProperties.CorrelationId;

                if (!string.IsNullOrWhiteSpace(correlationId))
                {
                    if (_callbackMapper.TryRemove(correlationId, out var tcs))
                    {
                        var body = ea.Body.ToArray();
                        var answer = Encoding.UTF8.GetString(body);
                        tcs.TrySetResult(answer);
                    }
                }

                await Task.CompletedTask;
            };

            await _channel.BasicConsumeAsync(queue: replyQueueName, autoAck: true, consumer: consumer);
        }

        public async Task<string> CallAsync(string routingKey, string uid, CancellationToken cancellationToken = default)
        {
            if (_channel is null)
            {
                throw new InvalidOperationException();
            }

            string correlationId = Guid.NewGuid().ToString();
            var basicProps = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = replyQueueName
            };

            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
            _callbackMapper.TryAdd(correlationId, tcs);

            var messageBytes = Encoding.UTF8.GetBytes(uid);
            await _channel.BasicPublishAsync(exchange: string.Empty, routingKey: routingKey, mandatory: true, basicProperties: basicProps, body: messageBytes);

            using CancellationTokenRegistration ctr = cancellationToken.Register(() =>
            {
                _callbackMapper.TryRemove(correlationId, out _);
                tcs.SetCanceled();
            });

            return await tcs.Task;
        }
    }
}
