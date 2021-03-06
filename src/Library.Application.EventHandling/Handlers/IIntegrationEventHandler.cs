﻿using System.Threading;
using System.Threading.Tasks;

namespace Library.Application.EventHandling.Handlers
{
    public interface IIntegrationEventHandler<in T> where T: IIntegrationEvent
    {
        Task Handle(T integrationEvent, CancellationToken token);
    }
}