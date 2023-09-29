using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using LarsProjekt.Application;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using System.Security.Claims;
using LarsProjekt.Database.Repositories;

namespace LarsProjekt.Controllers;

public class LoginController : Controller
{
    private IUserRepository _userRepository;
    public LoginController(IUserRepository userRepository)
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
            try
            {
                var customer = _userRepository.GetAll().FirstOrDefault(x => x.Username == model.UserName);

                //throw new ArgumentException();

                await SignIn(customer);

                return RedirectToAction(nameof(UserController.Index), nameof(Domain.User));
            }
            catch (NullReferenceException exception)
            {
                ModelState.AddModelError("Model", "User doesnt exist");
                
            }
            //catch(Exception exception)
            //{
            //    ModelState.AddModelError("Model", "Unexpected error occured. Please try again later..");
            //
            //}            
        }

        return View("~/Views/Login/SignIn.cshtml", model);
    }

    protected async Task SignIn(User user)
    {
        var claims = new[] {
            new Claim(ClaimTypes.Name, user.Username),
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

