using System;
using System.Threading;
using Library.Infrastructure;
using Library.Messaging.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace Library.Presentation.MVC.EventHandlers
{
    public class BookRateChangedEventHandler : IMessageHandler<BookRateChanged>
    {
        private readonly IHubContext<BookEventsRHub> _hubContext;

        public BookRateChangedEventHandler(IHubContext<BookEventsRHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void Handle(BookRateChanged message)
        {
            _hubContext.Clients.All.SendAsync("bookRateChanged", message, CancellationToken.None).Wait();
        }
    }
}