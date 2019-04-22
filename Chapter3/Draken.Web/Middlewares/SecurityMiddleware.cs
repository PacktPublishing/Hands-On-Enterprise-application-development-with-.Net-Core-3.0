using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Draken.Web.Middlewares
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        public SecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Xss-Protection", "1");
            httpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            await _next(httpContext);
        }
    }
}