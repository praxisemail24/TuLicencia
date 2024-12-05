using Microsoft.AspNetCore.Http.Extensions;

namespace SmartLicencia
{
    public class LowerCaseUrlMiddleware
    {
        private readonly RequestDelegate _next;

        public LowerCaseUrlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var path = request.Path.ToString().ToLowerInvariant();

            if (request.Path != path)
            {
                var query = request.QueryString.HasValue ? request.QueryString.Value?.ToLowerInvariant() : string.Empty;
                var lowercaseUrl = UriHelper.BuildAbsolute(request.Scheme, new HostString(request.Host.Value.ToLowerInvariant()), path, query);
                context.Response.Redirect(lowercaseUrl);
                return;
            }

            await _next(context);
        }
    }
}
