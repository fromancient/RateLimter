namespace RateLimiter.Core
{
    /// <summary>
    /// Implementation details for the rate limiter
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// The number of permits allowed by the rate limiter.
        /// </summary>
        int Permits { get; set; }

        /// <summary>
        /// The total interval in seconds in which a given number of permits are allowed
        /// e.g. 100 permits per 3600 seconds.
        /// </summary>
        int Seconds { get; set; }

        /// <summary>
        /// The implementation of the rate limiting strategy used to determine if a request should be permitted e.g. token bucket
        /// </summary>
        IRateLimiterStrategy Strategy { get; set; }
    }
}