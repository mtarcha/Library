using Library.Domain.Common;
using System;

namespace Library.Domain.Events
{
    public sealed class BookRateChangedEvent : IDomainEvent
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