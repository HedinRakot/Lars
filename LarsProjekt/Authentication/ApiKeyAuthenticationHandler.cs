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

        var valuesSection = _configuration.GetSection("Authentication:Users");
        foreach (IConfigurationSection section in valuesSection.GetChildren())
        {
            var apiKey = section.GetValue<string>("Key");
            var name = section.GetValue<string>("Name");

            if (key == apiKey)
            {
                var claims = new[] {
                new Claim(ClaimTypes.Name, name),
            };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var identities = new List<ClaimsIdentity> { identity }; // für mehrere Identities
                var principal = new ClaimsPrincipal(identities);
                var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

                return AuthenticateResult.Success(ticket);
            }
        }

        //var authSection = _configuration.GetSection(ApiKeyAuthenticationScheme.ApiKeySectionName);
        //var apiList = new List<string>();
        //foreach (var section in authSection.GetChildren())
        //{
        //    apiList.Add(section.Value);
        //}
        //var list = GetAppUsersFromAppSettings(_configuration);

        //var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //var apiKey = _configuration.GetValue<List<string>>(ApiKeyAuthenticationScheme.ApiKeySectionName);

        return AuthenticateResult.Fail("No Api key or key is wrong");
    }

    //public static IEnumerable<AppUser> GetAppUsersFromAppSettings(IConfiguration configuration)
    //{
    //    IConfigurationSection usersSection = configuration.GetSection(ApiKeyAuthenticationScheme.ApiKeySectionName);
    //    IEnumerable<IConfigurationSection> usersArray = usersSection.GetChildren();

    //    return usersArray.Select(configSection =>
    //        new AppUser
    //        (
    //            Name: configSection["Name"]!.ToString(),
    //            Key: configSection["Key"]!.ToString()
    //        ));
    //}

    //public static List<AppUser>? GetAppUsersFromAppSettings(IConfiguration configuration)
    //{
    //    return configuration.GetSection(ApiKeyAuthenticationScheme.ApiKeySectionName).Get<List<AppUser>>();
    //}
}
