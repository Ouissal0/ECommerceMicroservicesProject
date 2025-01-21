using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerce.SharedLibrary.Middleware
{
    public class ListenToOnlyApiGateway
    {
        private readonly RequestDelegate _next;

        public ListenToOnlyApiGateway(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Host.Host == "172.20.10.7" || context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var signedHeader = context.Request.Headers["Api-Gateway"];
            if (signedHeader.FirstOrDefault() is null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
                return;
            }

            await _next(context);
        }

    }
}
