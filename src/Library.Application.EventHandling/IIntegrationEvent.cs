using System;

namespace Library.Application.EventHandling
{
    public interface IIntegrationEvent
    {
        DateTime RaiseTime { get; }
    }
}