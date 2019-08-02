using Library.Domain.Events;
using MediatR;

namespace Library.Infrastucture.EventDispatching.MediatR
{
    public class DomainEventToNotificationConverter : IDomainEventVisitor<INotification>
    {
        public INotification VisitBookRateChangedEvent(BookRateChanged bookRateChangedEvent)
        {
            return new BookRateChangedNotification(bookRateChangedEvent.BookId, bookRateChangedEvent.Rate);
        }
    }
}