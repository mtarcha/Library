using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public interface IMessageHandler<in TMessage> where TMessage : class
    {
        void Handle(TMessage message);
    }
}