using System;
using System.Threading;
using System.Threading.Tasks;
using EasyCaching.Core;

namespace Library.Infrastructure.Cache.Redis
{
    public sealed class DistributedCache : IDistributedCache
    {
        private const string KeyPrefix = "Library_";
        private readonly IEasyCachingProvider _provider;

        public DistributedCache(IEasyCachingProvider provider)
        {
            _provider = provider;
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token)
        {
            var cacheItem = await _provider.GetAsync<T>(KeyPrefix + key);
            return cacheItem.Value;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan absoluteExpiration, CancellationToken token)
        {
            await _provider.SetAsync(KeyPrefix + key, value, absoluteExpiration);
        }

        public async Task ClearCache(CancellationToken token)
        {
            await _provider.RemoveByPrefixAsync(KeyPrefix);
        }
    }
}