using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                var userFromDb = _userRepository.GetByName(model.UserName);
                if (model.Password == userFromDb.Password)
                {
                    try
                    {
                        await SignIn(userFromDb);
                        return RedirectToAction(nameof(UserController.CreateEditAddress), nameof(Domain.User));

                    }
                    catch (NullReferenceException x)
                    {

                        AddError();
                    }
                }
                else { AddError(); }               

            }
            catch (NullReferenceException x)
            {
                AddError();
            }
        }

        return View("~/Views/Login/SignIn.cshtml", model);
    }

    private void AddError()
    {
        ModelState.AddModelError("Model", "Incorrect username or password");
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
            ExpiresUtc = DateTimeOffset.Now.AddDays(1)
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

