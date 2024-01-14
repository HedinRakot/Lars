//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Options;
//using System.Security.Claims;
//using System.Text.Encodings.Web;

//namespace LarsProjekt.Authentication;

//internal class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//{
//    private readonly IConfiguration _configuration;
//    public ApiKeyAuthenticationHandler(IConfiguration configuration, IOptionsMonitor<AuthenticationSchemeOptions> options,
//        ILoggerFactory logger,
//        UrlEncoder encoder,
//        ISystemClock clock) :
//        base(options, logger, encoder, clock)
//    {
//        _configuration = configuration;
//    }

//    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//    {
//        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

//        Request.Headers.TryGetValue(ApiKeyAuthenticationScheme.ApiKeyHeaderName, out var key);
//        var apiKey = _configuration.GetValue<string>(ApiKeyAuthenticationScheme.ApiKeySectionName);

//        if (key == apiKey)
//        {
//            var claims = new[] {
//                new Claim(ClaimTypes.Name, env),
//            };

//            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//            //var identities = new List<ClaimsIdentity> { identity }; // für mehrere Identities
//            //var principal = new ClaimsPrincipal(identities);

//            var principal = new ClaimsPrincipal(identity);
//            var ticket = new AuthenticationTicket(principal, ApiKeyAuthenticationScheme.DefaultScheme);

//            return AuthenticateResult.Success(ticket);
//        }

//        return AuthenticateResult.Fail("No Api key or key is wrong");
//    }
//}
