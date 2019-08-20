using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Library.Presentation.MVC.EventHandlers
{
    public class BookRateChangedEventHandler
    {
        private readonly IHubContext<BookEventsRHub> _hubContext;

        public BookRateChangedEventHandler(IHubContext<BookEventsRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        //todo : fix 
        public Task Handle(object notification, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync("bookRateChanged", notification, cancellationToken);
        }
    }
}