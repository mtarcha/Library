using MediatR;
using System;

namespace Library.Domain.Common
{
    public interface IDomainEvent : INotification
    {
        DateTime RaiseTime { get; }
    }
}