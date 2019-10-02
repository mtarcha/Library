using System;

namespace Library.Messaging.Contracts
{
    public sealed class BookRateChanged
    {
        public double Rate { get; set; }

        public Guid BookId { get; set; }

        public DateTime RaiseTime { get; set; }
    }
}