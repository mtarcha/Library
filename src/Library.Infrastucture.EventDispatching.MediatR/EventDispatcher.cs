using System.Collections.Concurrent;
using Library.Domain.Events;
using MediatR;

namespace Library.Infrastucture.EventDispatching.MediatR
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;
        private readonly DomainEventToNotificationConverter _domainEventToNotificationConverter;
        
        private readonly ConcurrentQueue<INotification> _deferredNotifications;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
            _domainEventToNotificationConverter = new DomainEventToNotificationConverter();
           
            _deferredNotifications = new ConcurrentQueue<INotification>();
        }

        public void DispatchDeferred<T>(T domainEvent) where T : IDomainEvent
        {
            var notification = domainEvent.Visit(_domainEventToNotificationConverter);
            _deferredNotifications.Enqueue(notification);
        }

        public void DispatchImmediately<T>(T domainEvent) where T : IDomainEvent
        {
            var notification = domainEvent.Visit(_domainEventToNotificationConverter);
            _mediator.Publish(notification);
        }

        public void RaiseDeferredEvents()
        {
            while (_deferredNotifications.TryDequeue(out var notification))
            {
                _mediator.Publish(notification);
            }
        }
    }
}