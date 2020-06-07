using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public interface IMessageService
    {
        //IDisposable Subscribe<TMessage>(Action<TMessage> messageHandler);

        Task SendAsync<TMessage>(TMessage message, CancellationToken token) where TMessage : class;
    }
}