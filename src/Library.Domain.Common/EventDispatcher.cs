using System.Collections.Concurrent;
using MediatR;

namespace Library.Domain.Common
{
    public class EventDispatcher
    {
        private readonly IMediator _mediator;       
        private readonly ConcurrentQueue<IDomainEvent> _deferredEvents;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;           
            _deferredEvents = new ConcurrentQueue<IDomainEvent>();
        }

        public void DispatchDeferred<T>(T domainEvent) where T : IDomainEvent
        {
            _deferredEvents.Enqueue(domainEvent);
        }

        public void DispatchImmediately<T>(T domainEvent) where T : IDomainEvent
        {
            _mediator.Publish(domainEvent);
        }

        public void RaiseDeferredEvents()
        {
            while (_deferredEvents.TryDequeue(out var domainEvent))
            {
                _mediator.Publish(domainEvent);
            }
        }
    }
}