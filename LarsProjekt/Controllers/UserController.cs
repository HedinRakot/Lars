using LarsProjekt.Application;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LarsProjekt.Controllers;

public class UserController : Controller
{
    private UserRepository _userRepository;
    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        var list = new List<UserModel>();
        foreach (var user in _userRepository.Users)
        {
            list.Add(new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Description = user.Description,
                Password = user.Password,
            });
        }

        return View(list);
    }

    public IActionResult Details(long id)
    {
        var user = _userRepository.Users.FirstOrDefault(o => o.Id == id);
        //TODO if (user == null)
        //{
        //    throw new Exception("User not found");
        //}

        var model = new UserModel
        {
            Id = user.Id,
            Name = user.Name,
            Description = user.Description,
            Password = user.Password
        };

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
        var user = _userRepository.Users.FirstOrDefault(o => o.Id == model.Id);
        user.Password = model.Password;

        return RedirectToAction(nameof(Details), new { Id = model.Id });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(UserModel model)
    {
        _userRepository.Users.Add(new Domain.User());
        //_userRepository.Users.Add(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Edit(UserModel model)
    {
        var user = _userRepository.Users.FirstOrDefault(o => o.Id == model.Id);
        user.Name = model.Name;
        user.Description = model.Description;

        return RedirectToAction(nameof(Index), new { Id = model.Id });

    }
}


