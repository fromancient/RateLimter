namespace RateLimiter.Core
{
    public class RateLimiterService : IRateLimiter
    {
        public int Permits { get; set; }
        public int Seconds { get; set; }
        public IRateLimiterStrategy Strategy { get; set; }

        public RateLimiterService(int permits, int seconds, IRateLimiterStrategy strategy)
        {
            this.Permits = permits;
            this.Seconds = seconds;
            this.Strategy = strategy;
        }
    }
}