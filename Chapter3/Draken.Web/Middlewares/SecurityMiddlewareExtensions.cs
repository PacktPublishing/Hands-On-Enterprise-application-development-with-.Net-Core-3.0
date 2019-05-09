using Draken.Web.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SecurityMiddlewareExtensions
    {
        public static IApplicationBuilder UseOWASPSecurity(this IApplicationBuilder applicationBuilder)
            => applicationBuilder.UseMiddleware<SecurityMiddleware>();
    }
}

