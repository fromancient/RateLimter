using Microsoft.AspNetCore.Builder;
using RateLimiter.Core;

namespace RateLimiter.Middleware
{
    public static class RateLimiterMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiter(this IApplicationBuilder builder, IRateLimiter service)
        {
            return builder.UseMiddleware<RateLimiterMiddleware>(service);
        }
    }
}