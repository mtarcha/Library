using MediatR;

namespace Library.Domain.Common
{
    public interface IDomainEventHandler<in T> : INotificationHandler<T> where T : IDomainEvent
    {
    }
}