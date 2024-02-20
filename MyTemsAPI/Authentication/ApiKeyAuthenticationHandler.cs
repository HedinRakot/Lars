using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using MyTemsAPI.Domain;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace MyTemsAPI.Authentication;

internal class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ApiAuthOptions _options;
    public ApiKeyAuthenticationHandler(IOptions<ApiAuthOptions> authOptions, IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) :
        base(options, logger, encoder, clock)
    {
        _options = authOptions.Value;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(ApiKeyAuthenticationScheme.ApiKeyHeaderName, out var key);

        List<AppUser> list = new();
        AppUser user = _options.Users.FirstOrDefault(x => x.Key == key);
        if (user != null)
        {
            list.Add(new AppUser { Key = user.Key, Name = user.Name });
            if (key == user.Key)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user.Name) };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var identities = new List<ClaimsIdentity> { identity };
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

                // AppUser in neuer DbTabelle speichern, um Zugriff auf Api zu dokumentieren

                return AuthenticateResult.Success(ticket);
            }
        }        
        return AuthenticateResult.Fail("API Key is wrong or missing");
    }

}
