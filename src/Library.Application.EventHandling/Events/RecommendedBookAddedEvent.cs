using System;
using Library.Application.EventHandling.Handlers;

namespace Library.Application.EventHandling.Events
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