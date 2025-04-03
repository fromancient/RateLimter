using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter.Core;
using RateLimiter.Cache;

namespace RateLimiter.Strategy
{
    public class TokenBucketStrategy : IRateLimiterStrategy
    {
        private readonly IRateLimiterCache _cache;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly bool _isSlidingWindow;

        public TokenBucketStrategy(IRateLimiterCache cache, bool isSlidingWindow = false)
        {
            _cache = cache;
            _isSlidingWindow = isSlidingWindow;
        }

        public bool PermitRequest(string key, IRateLimiter service)
        {
            _semaphore.Wait();
            try
            {
                TokenBucketRecord result = (TokenBucketRecord)_cache.Get(key) ?? ResetInterval(key, service.Permits);
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (_isSlidingWindow)
                {
                    // Sliding window: Check within the last 24 hours
                    var intervalStart = result.StartInterval;
                    var intervalEnd = intervalStart + 86400; // 24 hours in seconds
                    if (currentTime > intervalEnd)
                    {
                        result = ResetInterval(key, service.Permits);
                    }
                }
                else
                {
                    // Absolute window: Reset at 00:00 UTC
                    var intervalStart = GetStartOfDay();
                    var intervalEnd = intervalStart + service.Seconds;

                    if (currentTime > intervalEnd)
                    {
                        result = ResetInterval(key, service.Permits);
                    }
                }

                if (result.Tokens <= 0)
                {
                    return false;
                }

                _cache.Set(key, new TokenBucketRecord
                {
                    StartInterval = result.StartInterval,
                    Tokens = result.Tokens - 1
                });
                return true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public long SecondsUntilNextPermittedRequest(string key, int seconds)
        {
            _semaphore.Wait();
            try
            {
                TokenBucketRecord result = (TokenBucketRecord)_cache.Get(key);
                if (result == null) return 0;

                var intervalStart = result.StartInterval;
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                var intervalEnd = intervalStart + seconds;
                return Math.Max(0, intervalEnd - currentTime);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private TokenBucketRecord ResetInterval(string key, int tokens)
        {
            var record = new TokenBucketRecord
            {
                StartInterval = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Tokens = tokens
            };
            _cache.Set(key, record);
            return record;
        }
        
        private long GetStartOfDay()
        {
            var utcNow = DateTimeOffset.UtcNow;
            var startOfDay = new DateTimeOffset(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, TimeSpan.Zero);
            return startOfDay.ToUnixTimeSeconds();
        }
    }
}
