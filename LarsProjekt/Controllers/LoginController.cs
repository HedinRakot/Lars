using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LarsProjekt.Application;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using System.Security.Claims;
using LarsProjekt.Controllers;

namespace LarsProjekt.Controllers;

public class LoginController : Controller
{
    private UserRepository _userRepository;
    public LoginController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        return View(new LoginModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var customer = _userRepository.Users.FirstOrDefault(x => x.Name == model.UserName);

            await SignIn(customer);

            return RedirectToAction(nameof(UserController.Index), nameof(Domain.User));
        }

        return View("~/Views/Login/SignIn.cshtml", model);
    }

    protected async Task SignIn(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Name),
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties()
        {
            IsPersistent = true,
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.Now.AddDays(1),
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);
    }

    [HttpGet]
    public async Task<IActionResult> SignOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(SignIn));
    }
}

