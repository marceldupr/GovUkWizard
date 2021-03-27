using Microsoft.AspNetCore.Http;

namespace Dfe.Wizard.Prototype.Application
{
    public static class Context
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static HttpContext Current => _httpContextAccessor.HttpContext;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void Configure(HttpContext context)
        {
            _httpContextAccessor = new HttpContextAccessor {HttpContext = context};
        }
    }
}
