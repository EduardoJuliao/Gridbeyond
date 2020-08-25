using System;
using GridBeyond.Domain.Interfaces.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace GridBeyond.Domain.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _cache;

        public CacheRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public bool GetCache<T>(string name, out T cacheEntry)
        {
            return _cache.TryGetValue(name, out cacheEntry);
        }

        public void SetOrUpdate<T>(T data, string name)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(86400));

            _cache.Set(name, data, cacheEntryOptions);
        }
    }
}
