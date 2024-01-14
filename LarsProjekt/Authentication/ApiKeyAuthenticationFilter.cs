using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Authentication
{
    public class ApiKeyAuthenticationFilter : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public ApiKeyAuthenticationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue(ApiKeyAuthenticationScheme.ApiKeyHeaderName, out var key))
            {
                context.Result = new UnauthorizedObjectResult("unauthorized");
                return;
            }
            var apiKey = _configuration.GetValue<string>(ApiKeyAuthenticationScheme.ApiKeySectionName);
            if(!apiKey.Equals(key))
            {
                context.Result = new UnauthorizedObjectResult("unauthorized");
                return;
            }
        }
    }
}
