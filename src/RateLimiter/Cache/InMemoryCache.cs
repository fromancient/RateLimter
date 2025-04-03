using System.Runtime.Caching;

namespace RateLimiter.Cache
{
    public class InMemoryCache : IRateLimiterCache
    {
        private readonly MemoryCache _cache;

        public InMemoryCache(MemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public void Set(string key, object value)
        {
            _cache.Set(new CacheItem(key, value), new CacheItemPolicy());
        }
    }
}