using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Library.Infrastucture.EventDispatching.MediatR
{
    public class BookRateChangedNotificationDispatcher : INotificationHandler<BookRateChangedNotification>
    {
        private readonly IHubContext<BookRateChangedHub> _hubContext;

        public BookRateChangedNotificationDispatcher(IHubContext<BookRateChangedHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public Task Handle(BookRateChangedNotification notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("bookRateChanged", notification, cancellationToken);
        }
    }
}