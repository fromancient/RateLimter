using System.Runtime.Caching;
using RateLimiter.Core;
using RateLimiter.Strategy;
using RateLimiter.Cache;
using Xunit;

namespace RateLimiterTests
{
    public class TokenBucketStrategyTests
    {
        /// <summary>
        /// Validate that the token bucket strategy rejects a request 
        /// </summary>
        [Fact]
        public void NoTokensAvailable_DropsRequest()
        {
            // Arrange
            var service = new RateLimiterService(
                100,
                3600,
                new TokenBucketStrategy(new InMemoryCache(new MemoryCache("TokenBucketStrategyTest")))
            );

            // Act
            for (int i = 0; i < 101; i++)
            {
                service.Strategy.PermitRequest("test", service);
            }

            // Assert
            Assert.False(service.Strategy.PermitRequest("test", service));
        }
    }
}

