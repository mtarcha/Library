using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Infrastructure.Cache
{
    public interface IDistributedCache
    {
        Task<T> GetAsync<T>(string key, CancellationToken token);

        Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, CancellationToken token);

        Task ClearCache(CancellationToken token);
    }
}