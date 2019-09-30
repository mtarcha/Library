using System;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;

namespace Library.Infrastructure.Messaging.RabbitMq
{
    internal sealed class MessageService : IMessageService, IDisposable
    {
        private readonly IBus _messageBus;

        public MessageService(IBus messageBus)
        {
            _messageBus = messageBus;
        }
       
        public async Task SendAsync<TMessage>(TMessage message, CancellationToken token) where TMessage : class
        {
            await _messageBus.PublishAsync(message);
        }

        public void Dispose()
        {
            _messageBus?.Dispose();
        }
    }
}