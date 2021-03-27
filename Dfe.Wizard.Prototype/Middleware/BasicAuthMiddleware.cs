using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Dfe.Wizard.Prototype.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Dfe.Wizard.Prototype.Middleware
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BasicAuthOptions _basicAuthOptions;

        public BasicAuthMiddleware(
            RequestDelegate next,
            IOptions<BasicAuthOptions> basicAuthOptions)
        {
            _next = next;
            _basicAuthOptions = basicAuthOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization"))
            {
                Challenge(context);

                return;
            }

            string username;
            string password;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                username = credentials[0];
                password = credentials[1];
            }
            catch
            {
                Challenge(context);

                return;
            }

            if (username != _basicAuthOptions.UserName || password != _basicAuthOptions.Password)
            {
                Challenge(context);

                return;
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }

        private void Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Append(HeaderNames.WWWAuthenticate, "Basic realm=\"" + context.Request.Host + "\"");
        }
    }
}
