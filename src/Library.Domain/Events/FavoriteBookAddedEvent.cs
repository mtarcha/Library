using System;
using Library.Domain.Common;

namespace Library.Domain.Events
{
    public class FavoriteBookAddedEvent : IDomainEvent
    {
        public FavoriteBookAddedEvent(Guid userId, Guid bookId)
        {
            RaiseTime = DateTime.Now;
            UserId = userId;
            BookId = bookId;
        }

        public DateTime RaiseTime { get; }

        public Guid UserId { get; }

        public Guid BookId { get; }
    }
}