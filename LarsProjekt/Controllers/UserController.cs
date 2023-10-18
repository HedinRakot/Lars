using LarsProjekt.Database.Repositories;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class UserController : Controller
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository, IAddressRepository addressRepository)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
    }
    public IActionResult Details(long id)
    {
        var user = _userRepository.Get(id);
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
    public IActionResult ChangePassword(ChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _userRepository.Get(model.Id);
            if (model.Password == model.PasswordRepeat)
            {
                user.Password = model.Password;
                return RedirectToAction(nameof(CreateEditAddress), new { Id = model.Id });
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
    public IActionResult CreateEdit()
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var users = _userRepository.GetAll();
        foreach (var user in users)
        {
            if (signedInUser != null && signedInUser.Id == user.Id)
            {
                var model = user.ToModel();
                return View(model);
            }
        }
        return View(new UserModel());
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult CreateEdit(UserModel model)
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);

        if (model.Id == signedInUser.Id)
        {
            if (ModelState.IsValid)
            {
                var user = model.ToDomain();
                _userRepository.Update(user);
                return RedirectToAction(nameof(CreateEditAddress));
            }
            else { return View(); }
        }
        else
        {
            if (ModelState.IsValid)
            {
                var user = model.ToDomain();
                _userRepository.Add(user);
                return RedirectToAction(nameof(CreateEditAddress));
            }
            else return View();
        }

    }

    [HttpDelete]
    public IActionResult Delete(long id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var model = _userRepository.Get(id);
        _userRepository.Delete(model);
        return Ok(new { success = "true" });
    }

    [HttpGet]
    public IActionResult CreateEditAddress()
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var users = _userRepository.GetAll();
        foreach (var user in users)
        {
            if (signedInUser.Id == user.Id)
            {
                var address = _addressRepository.Get(user.AddressId);
                var model = address.ToModel();
                return View(model);
            }
        }

        return View(new AddressModel());
    }

    [HttpPost]
    public IActionResult CreateEditAddress(AddressModel model)
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);
        var users = _userRepository.GetAll();
        foreach (var user in users)
        {
            if (user.Id == signedInUser.Id)
            {
                if (ModelState.IsValid)
                {
                    var address = model.ToDomain();
                    _addressRepository.Update(address);
                    return RedirectToAction(nameof(CreateEditAddress));
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var address = model.ToDomain();
                    _addressRepository.Add(address);
                    return RedirectToAction(nameof(CreateEditAddress));
                }                
            }
        } return View(model);
    }
}

    