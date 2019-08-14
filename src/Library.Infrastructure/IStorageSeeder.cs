using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public interface IStorageSeeder
    {
        Task SeedAsync(CancellationToken token);
    }
}