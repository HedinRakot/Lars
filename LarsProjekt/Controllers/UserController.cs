using LarsProjekt.Application;
using LarsProjekt.Database;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;

public class UserController : Controller
{

    private readonly IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        var list = new List<UserModel>();
        foreach (var user in _userRepository.GetAll())
        {
            list.Add(user.ToModel());
        }
        return View(list);
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
                return RedirectToAction(nameof(Index), new { Id = model.Id });
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
    public IActionResult CreateEdit(int id)
    {
        if (id == 0)
        {
            return View(new UserModel());
        }
        else
        {
            var user = _userRepository.Get(id);
            var model = user.ToModel();
            return View(model);
        }

    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult CreateEdit(UserModel model)
    {
        if (model.Id == 0)
        {
            if (ModelState.IsValid)
            {
                var user = model.ToDomain();                
                _userRepository.Add(user);
                return RedirectToAction(nameof(Index));
            }
            else { return View(); }
        }
        else
        {
            if (ModelState.IsValid)
            {
                var user = model.ToDomain();
                _userRepository.Update(user);
                return RedirectToAction(nameof(Index));
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
}

