using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.Core
{
    public interface IStorageSeeder
    {
        Task SeedAsync(CancellationToken token);
    }
}