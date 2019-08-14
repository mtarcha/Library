using System.Threading;
using System.Threading.Tasks;
using Library.Application.EventHandling.Events;
using Library.Application.EventHandling.Handlers;
using Microsoft.AspNetCore.SignalR;

namespace Library.Presentation.MVC.EventHandlers
{
    public class BookRateChangedEventHandler : IIntegrationEventHandler<BookRateChangedEvent>
    {
        private readonly IHubContext<BookEventsRHub> _hubContext;

        public BookRateChangedEventHandler(IHubContext<BookEventsRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(BookRateChangedEvent notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("bookRateChanged", notification, cancellationToken);
        }
    }
}