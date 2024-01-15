using LarsProjekt.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace LarsProjekt.Authentication;

internal class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;
    public ApiKeyAuthenticationHandler(IConfiguration configuration, IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) :
        base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        Request.Headers.TryGetValue(ApiKeyAuthenticationScheme.ApiKeyHeaderName, out var key);

        List<AppUser> list = new List<AppUser>();
        var authSection = _configuration.GetSection(ApiKeyAuthenticationScheme.ApiKeySectionName);
        foreach (IConfigurationSection section in authSection.GetChildren())
        {
            
            var apiKey = section.GetValue<string>("Key");
            var name = section.GetValue<string>("Name");
            list.Add(new AppUser(name, apiKey));

            if (key == apiKey)
            {
                var claims = new[] {
                new Claim(ClaimTypes.Name, name)
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var identities = new List<ClaimsIdentity> { identity }; // für mehrere Identities
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

                return AuthenticateResult.Success(ticket);
            }
        }

        return AuthenticateResult.Fail("No Api key or key is wrong");
    }
}
