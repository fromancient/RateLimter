using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RateLimiter.Core;

namespace RateLimiter.Middleware
{
    public class RateLimiterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRateLimiter _service;

        public RateLimiterMiddleware(RequestDelegate next, IRateLimiter service)
        {
            _next = next;
            _service = service;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract the IP address from the request. 
            // The IP address is used to identify a machine making a request to the service.
            var key = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();

            // Determine if the request should be permitted. If the rate limited has been exceeded, drop the request.
            if (!_service.Strategy.PermitRequest(key, _service))
            {
                // Send a 429 response.
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                // Notify the user how many seconds they must wait before submitting another request.
                var seconds = _service.Strategy.SecondsUntilNextPermittedRequest(key, _service.Seconds).ToString();

                await context.Response.WriteAsync($"Rate limit exceeded. Try again in {seconds} second(s)");
            }
            else
            {
                // If the request was permitted, pass the request on to the next stage of the pipeline
                await _next(context);
            }
        }
    }
}