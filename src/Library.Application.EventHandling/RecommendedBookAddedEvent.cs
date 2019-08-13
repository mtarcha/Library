using System;

namespace Library.Application.EventHandling
{
    public class RecommendedBookAddedEvent : IIntegrationEvent
    {
        public RecommendedBookAddedEvent(Guid userId, Guid bookId)
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