using System;
using Library.Application.EventHandling.Handlers;

namespace Library.Application.EventHandling.Events
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

        public override string ToString()
        {
            return $"New book {BookId} rate: {Rate}";
        }
    }
}