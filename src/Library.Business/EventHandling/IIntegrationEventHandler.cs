using System.Threading;
using System.Threading.Tasks;

namespace Library.Business.EventHandling
{
    public interface IIntegrationEventHandler<in T> where T: IIntegrationEvent
    {
        Task Handle(T integrationEvent, CancellationToken token);
    }
}