using System.Threading;
using System.Threading.Tasks;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Library.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Library.Api.Utility
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

            await _messageService.SendAsync(integrationEvent, token);
        }
    }
}