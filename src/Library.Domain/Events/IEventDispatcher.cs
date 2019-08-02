namespace Library.Domain.Events
{
    public interface IEventDispatcher
    {
        void DispatchDeferred<T>(T domainEvent) where T : IDomainEvent;

        void DispatchImmediately<T>(T domainEvent) where T : IDomainEvent;

        void RaiseDeferredEvents();
    }
}