using System;

namespace Library.Business.EventHandling
{
    public interface IIntegrationEvent
    {
        DateTime RaiseTime { get; }
    }
}