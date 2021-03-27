using Microsoft.AspNetCore.Builder;

namespace Dfe.Wizard.Prototype.Middleware
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBasicAuth(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}
