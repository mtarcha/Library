using System;
using System.Linq;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Messaging.RabbitMq
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, string connectionString)
        {
            var messageHandlers = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                .Where(x =>x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMessageHandler<>)));

            foreach (var messageHandler in messageHandlers)
            {
                services.AddScoped(messageHandler);
            }

            services.AddSingleton(RabbitHutch.CreateBus(connectionString));
            services.AddHostedService<MessagesDispatcher>();
            services.AddSingleton<IMessageService, MessageService>();
            
            return services;
        }
    }
}