using System;

namespace Library.Application.EventHandling.Handlers
{
    public interface IIntegrationEvent
    {
        DateTime RaiseTime { get; }
    }
}