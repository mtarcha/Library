using System.Threading;
using System.Threading.Tasks;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Library.Infrastructure;
using Library.Messaging.Contracts;
using Microsoft.Extensions.Logging;

namespace Library.Api.Handlers.EventHandlers
{
    public class BookRateChangedEventHandler : IIntegrationEventHandler<BookRateChangedEvent>
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<BookRateChangedEventHandler> _logger;

        public BookRateChangedEventHandler(IMessageService messageService, ILogger<BookRateChangedEventHandler> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        public async Task Handle(BookRateChangedEvent integrationEvent, CancellationToken token)
        {
            _logger.LogInformation($"Book rate changed event: {integrationEvent}");

            var message = new BookRateChanged
            {
                BookId = integrationEvent.BookId,
                Rate = integrationEvent.Rate,
                RaiseTime = integrationEvent.RaiseTime
            };

            await _messageService.SendAsync(message, token);
        }
    }
}