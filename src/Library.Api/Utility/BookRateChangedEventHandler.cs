using System.Threading;
using System.Threading.Tasks;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Microsoft.Extensions.Logging;

namespace Library.Api.Utility
{
    public class BookRateChangedEventHandler : IIntegrationEventHandler<BookRateChangedEvent>
    {
        private readonly ILogger<BookRateChangedEventHandler> _logger;

        public BookRateChangedEventHandler(ILogger<BookRateChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(BookRateChangedEvent integrationEvent, CancellationToken token)
        {
            // todo: send event via RabbitMQ
            return Task.Run(() => _logger.LogInformation("Event"), token);
        }
    }
}