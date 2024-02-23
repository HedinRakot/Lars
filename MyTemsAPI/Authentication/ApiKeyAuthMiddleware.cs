using Microsoft.Extensions.Options;
using MyTemsAPI.Domain;
using System.Net;

namespace MyTemsAPI.Authentication;

public class ApiKeyAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiAuthOptions _options;
    public ApiKeyAuthMiddleware(RequestDelegate next, IOptions<ApiAuthOptions> authOptions)
    {
        _next = next;
        _options = authOptions.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(!context.Request.Headers.TryGetValue(ApiKeyAuthenticationScheme.ApiKeyHeaderName, out var key))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return;
        }

        AppUser user = _options.Users.FirstOrDefault(x => x.Key == key);

        if (!key.Equals(user.Key))
        {
            context.Response.StatusCode = (int)(HttpStatusCode.Unauthorized);
            return;
        }

        // AppUser spreichern um Zugriff zu dokumentieren

        await _next(context).ConfigureAwait(false);
    }
}
