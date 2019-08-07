namespace Library.Domain.Common
{
    public interface IEventDispatcher
    {
        void DispatchDeferred<T>(T domainEvent) where T : IDomainEvent;

        void DispatchImmediately<T>(T domainEvent) where T : IDomainEvent;

        void RaiseDeferredEvents();
    }
}