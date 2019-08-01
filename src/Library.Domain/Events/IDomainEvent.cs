using System;

namespace Library.Domain.Events
{
    public interface IDomainEvent
    {
        DateTime RaiseTime { get; }

        T Visit<T>(IDomainEventVisitor<T> visitor);
    }
}