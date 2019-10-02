using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.NonGeneric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Library.Infrastructure.Messaging.RabbitMq
{
    internal sealed class MessagesDispatcher : IHostedService
    {
        private readonly IBus _messageBus;
        private readonly IServiceProvider _services;

        private readonly List<IDisposable> _subscriptions;

        public MessagesDispatcher(IBus messageBus, IServiceProvider services)
        {
            _messageBus = messageBus;
            _services = services;

            _subscriptions = new List<IDisposable>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var messageHandlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMessageHandler<>)));

            foreach (var messageHandlerType in messageHandlers)
            {
                var argumentType = messageHandlerType
                    .GetInterfaces().First(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMessageHandler<>))
                    .GetGenericArguments().First();

                var subscription = _messageBus.Subscribe(
                    argumentType,
                    "Library",
                    message =>
                    {
                        using (var scope = _services.CreateScope())
                        {
                            var messageHandler = scope.ServiceProvider.GetRequiredService(messageHandlerType);
                            messageHandlerType.GetMethod("Handle").Invoke(messageHandler, new[] { message });
                        }
                    });

                _subscriptions.Add(subscription);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}