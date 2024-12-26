 using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;

namespace WebApplication2.Helpers
{
    public class CacheHelper
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CacheHelper> _logger;

        // Constructor to inject IMemoryCache and ILogger for optional logging
        public CacheHelper(IMemoryCache cache, ILogger<CacheHelper> logger = null)
        {
            _cache = cache;
            _logger = logger;
        }

        // CachedLong: Caches a result indefinitely, using a delegate for dynamic cache retrieval
        public T CachedLong<T>(string cacheKey, Func<T> getItem)
        {
            return CacheResult(cacheKey, getItem, new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove
            });
        }

        // Cached: Caches a result for up to 5 minutes, with a delegate for dynamic cache retrieval
        public T Cached<T>(string cacheKey, Func<T> getItem)
        {
            return CacheResult(cacheKey, getItem, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        }

        // Generic method to manage caching using delegates and options
        private T CacheResult<T>(string cacheKey, Func<T> getItem, MemoryCacheEntryOptions cacheOptions)
        {
            if (!_cache.TryGetValue(cacheKey, out T cacheEntry))
            {
                _logger?.LogInformation($"Cache miss for key: {cacheKey}. Retrieving and caching new data.");

                // Retrieve the data and store it in cache
                cacheEntry = getItem();
                _cache.Set(cacheKey, cacheEntry, cacheOptions);
            }
            else
            {
                _logger?.LogInformation($"Cache hit for key: {cacheKey}");
            }

            return cacheEntry;
        }

        // Optional: Method to remove a cached item (by cacheKey) if needed
        public void Remove(string cacheKey)
        {
            _cache.Remove(cacheKey);
            _logger?.LogInformation($"Cache entry removed for key: {cacheKey}");
        }
    }
}
