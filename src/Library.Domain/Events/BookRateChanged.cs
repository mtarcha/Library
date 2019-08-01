using System;

namespace Library.Domain.Events
{
    public sealed class BookRateChanged : IDomainEvent
    {
        public BookRateChanged(double rate, Guid bookId)
        {
            Rate = rate;
            BookId = bookId;
            RaiseTime = DateTime.Now;
        }

        public double Rate { get; }

        public Guid BookId { get; }

        public DateTime RaiseTime { get; }

        public T Visit<T>(IDomainEventVisitor<T> visitor)
        {
            return visitor.VisitBookRateChangedEvent(this);
        }
    }
}