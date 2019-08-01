using Library.Domain.Events;
using MediatR;

namespace Library.Infrastucture.EventDispatching.MediatR
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        void DispatchDeferred<T>(T domainEvent) where T : IDomainEvent
        {
            //_mediator.Publish()
        }

        void DispatchImmediately<T>(T domainEvent) where T : IDomainEvent
        {
            
        }

        void RaiseDeferredEvent()
        {
            
        }
    }
}