using System;

namespace Library.Application.EventHandling
{
    public sealed class BookRateChangedEvent : IIntegrationEvent
    {
        public BookRateChangedEvent(double rate, Guid bookId)
        {
            Rate = rate;
            BookId = bookId;
            RaiseTime = DateTime.Now;
        }

        public double Rate { get; }

        public Guid BookId { get; }

        public DateTime RaiseTime { get; }
    }
}