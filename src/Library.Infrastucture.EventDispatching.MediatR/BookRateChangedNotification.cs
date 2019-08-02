using System;
using MediatR;

namespace Library.Infrastucture.EventDispatching.MediatR
{
    public class BookRateChangedNotification : INotification
    {
        public BookRateChangedNotification(Guid bookId, double rate)
        {
            BookId = bookId;
            Rate = rate;
        }

        public Guid BookId { get; }

        public double Rate { get; }
    }
}