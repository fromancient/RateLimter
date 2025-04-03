namespace RateLimiter.Cache
{
    /// <summary>
    /// Implementation details for a cache used by the rate limiter.
    /// </summary>
    public interface IRateLimiterCache
    {
        object Get(string key);
        void Set(string key, object value);
    }
}