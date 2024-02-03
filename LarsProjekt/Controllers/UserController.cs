using LarsProjekt.Application;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LarsProjekt.Controllers;
public class UserController : Controller
{
    private readonly IAddressService _addressSerivce;
    private readonly IUserService _userService;
    public UserController(IUserService userService, IAddressService addressService)
    {        
        _userService = userService;
        _addressSerivce = addressService;
    }
    public async Task<IActionResult> Details(long id)
    {
        var user = await _userService.GetById(id);
        var model = user.ToModel();

        return View(model);
    }

    [HttpGet]
    public IActionResult ChangePassword(long id)
    {
        var model = new ChangePasswordModel
        {
            Id = id
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetById(model.Id);
            if (model.Password == model.PasswordRepeat)
            {
                user.Password = model.Password;
                return RedirectToAction(nameof(CreateEdit), new { Id = model.Id });
            }
            else
            {
                ModelState.AddModelError(nameof(ChangePasswordModel.PasswordRepeat), "Passwords do not match");
            }
        }

        return View(model);
    }
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> CreateEdit()
    {
        var signedInUser = await _userService.GetByName(HttpContext.User.Identity.Name);

        if( signedInUser != null )
        {
            var address = await _addressSerivce.GetById(signedInUser.AddressId);
            if (signedInUser != null)
            {
                UserRegistrationVM vm = new UserRegistrationVM()
                {
                    UserModel = signedInUser.ToModel(),
                    AddressModel = address.ToModel()
                };
                return View(vm);
            }
        }
        UserRegistrationVM vme = new UserRegistrationVM()
        {
            UserModel = new UserModel(),
            AddressModel = new AddressModel()
        };
        return View(vme);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateEditAsync(UserRegistrationVM model)
    {
        var signedInUser = await _userService.GetByName(HttpContext.User.Identity.Name);

        if (ModelState.IsValid)
        {
            if (signedInUser == null)
            {
                var user = model.UserModel.ToDomain();
                var address = model.AddressModel.ToDomain();
                user.Address = address;
                await _addressSerivce.Create(address);               
                await _userService.Create(user);                           

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

                return RedirectToAction(nameof(CreateEdit));
            }
            
        } else
        {
            ModelState.AddModelError("Model", "Please check your information");
        }

        // TODO Nutzernamen ändern
        if (model.UserModel.Id == signedInUser.Id)
        {
            var address = model.AddressModel.ToDomain();
            await _addressSerivce.Update(address);
            return RedirectToAction(nameof(CreateEdit));
        }

        return View(model);
    }
}

