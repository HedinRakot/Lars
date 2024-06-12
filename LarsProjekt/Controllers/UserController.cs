using Grpc.Net.Client;
using LarsProjekt.Application.IService;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserApi.Grpc.Protos;

namespace LarsProjekt.Controllers;
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {        
        _userService = userService;
    }

    public async Task<IActionResult> Details(long id)
    {
        var input = new GetByIdRequest { CustomerId = (int)id };
        var channel = GrpcChannel.ForAddress("https://localhost:7102");
        var client = new Customers.CustomersClient(channel);
        var reply = await client.GetByIdAsync(input);

        CustomerModel model = new()
        {
            Id = reply.Id,
            FirstName = reply.FirstName,
            LastName = reply.LastName,
            Email = reply.EMail,
            Password = reply.Password
        };

        return View(model);
    }



    // TODO CHANGE PASSWORD
    // CHANGE USERNAME

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> CreateEdit()
    {
        var signedInUser = await _userService.GetByNameWithAddress(HttpContext.User.Identity.Name);

        if( signedInUser != null )
        {
            
            if (signedInUser != null)
            {
                UserRegistrationVM vm = new()
                {
                    UserModel = signedInUser.ToModel(),
                    AddressModel = signedInUser.Address.ToModel()
                };
                return View(vm);
            }
        }
        UserRegistrationVM vme = new()
        {
            UserModel = new UserModel(),
            AddressModel = new AddressModel()
        };
        return View(vme);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateEdit(UserRegistrationVM model)
    {
        var signedInUser = await _userService.GetByNameWithAddress(HttpContext.User.Identity.Name);

        var user = model.UserModel.ToDomain();
        user.Address = model.AddressModel.ToDomain();

        if (ModelState.IsValid)
        {
            if (signedInUser == null)
            {                                      
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

        if (model.UserModel.Id == signedInUser.Id)
        {            
            await _userService.Update(user);
            return RedirectToAction(nameof(CreateEdit));
        }

        return View(model);
    }
}

