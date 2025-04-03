using RateLimiter.Core;

namespace RateLimiter
{
    /// <summary>
    /// Interface for implementing a rate limiting strategy.
    /// </summary>
    public interface IRateLimiterStrategy
    {
        /// <summary>
        /// Determine if a request should be permitted.
        /// <param name="key">Key used in the cache</param>
        /// <param name="service">Instance of the rate limiter service</param>
        /// <summary>
        bool PermitRequest(string key, IRateLimiter service);

        /// <summary>
        /// Notify the requestor how many seconds they have to wait before they can make a valid request.
        /// <param name="key">Key used in the cache</param>
        /// <param name="seconds">The total interval in seconds in which a given number of permits are allowed</param>
        /// </summary>
        long SecondsUntilNextPermittedRequest(string key, int seconds);
    }
}