using Google.Protobuf.WellKnownTypes;
using IdentityModel.Client;
using LarsProjekt.Application.IService;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LarsProjekt.Controllers;
public class LoginController : Controller
{    
    private IUserService _userService;
    private readonly ILogger<LoginController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    public LoginController(IUserService userService, ILogger<LoginController> logger, IHttpClientFactory httpClientFactory)
    {
        _userService = userService;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    //
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }


    //[HttpGet]
    //public IActionResult SignIn()
    //{
    //    return View(new LoginModel());
    //}

    //[HttpGet]
    //public async Task<TokenResponse> SignIn(string code)
    //{

    //    // call Auth Server to exchange the code by the token
    //    var httpClient = _httpClientFactory.CreateClient();
    //    var discoveryDoc = await httpClient.GetDiscoveryDocumentAsync(
    //        "https://localhost:7099/");

    //    // constructs the token request
    //    var authCodeRequest = new AuthorizationCodeTokenRequest()
    //    {
    //        Address = discoveryDoc.TokenEndpoint,
    //        Code = code,
    //        ClientId = "aspnetcoreweb", // indicates a code exchange
    //        ClientSecret = "secret",  // registered in Auth Server
    //        CodeVerifier = null,      // no pkce
    //        RedirectUri = "https://localhost:7099/gettokenfromcode" // same url
    //    };

    //    // request the token in exchange for the code
    //    var duendeResponse = await httpClient.RequestAuthorizationCodeTokenAsync(authCodeRequest);
    //    if (duendeResponse.IsError)
    //        throw new BadHttpRequestException(duendeResponse.Error);

    //    // return the entire response, which includes the access and id tokens
    //    return duendeResponse;
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> SignIn(LoginModel model)
    //{        

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            var userFromDb = await _userService.GetByNameWithAddress(model.UserName);
    //            if (model.Password == userFromDb.Password)
    //            {
    //                try
    //                {
    //                    await SignIn(userFromDb);
    //                    return RedirectToAction(nameof(UserController.CreateEdit), nameof(Domain.User));

    //                }
    //                catch (NullReferenceException x)
    //                {
    //                    _logger.LogError(x.Message);
    //                    AddError();
    //                }
    //            }
    //            else { AddError(); }               

    //        }
    //        catch (NullReferenceException x)
    //        {
    //            _logger.LogError(x.Message);
    //            AddError();
    //        }
    //    }

    //    return View("~/Views/Login/SignIn.cshtml", model);
    //}

    //private void AddError()
    //{
    //    ModelState.AddModelError("Model", "Incorrect username or password");
    //}

    //protected async Task SignIn(User user)
    //{
    //    var claims = new[] {
    //        new Claim(ClaimTypes.Name, user.Username),
    //    };

    //    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    //    var authProperties = new AuthenticationProperties()
    //    {
    //        IsPersistent = true,
    //        AllowRefresh = true,
    //        ExpiresUtc = DateTimeOffset.Now.AddDays(1)
    //    };

    //    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
    //}

    //[HttpGet]
    //public async Task<IActionResult> SignOut()
    //{
    //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    return RedirectToAction(nameof(SignIn));
    //}
}

