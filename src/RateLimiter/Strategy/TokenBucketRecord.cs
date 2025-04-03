namespace RateLimiter.Strategy
{
    public class TokenBucketRecord
    {
        /// <summary>
        /// The last time the token bucket was refilled.
        /// If the rate limiter has never encountered this client, this is set to the current time.
        /// </summary>
        public long StartInterval { get; set; }

        /// <summary>
        /// The total number of tokens available for the current client.
        /// </summary>
        public int Tokens { get; set; }
    }
}