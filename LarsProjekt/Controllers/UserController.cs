using LarsProjekt.Application;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;

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
        if (ModelState.IsValid)
        {
            var user = _userRepository.Users.FirstOrDefault(o => o.Id == model.Id);
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

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UserModel());
    }

    [HttpPost]
    public IActionResult Create(UserModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User();
            user.Name = model.Name;
            user.Description = model.Description;
            var maxId = _userRepository.Users.Max(o => o.Id);
            user.Id = maxId + 1;

            _userRepository.Users.Add(user);

            return RedirectToAction(nameof(Index));
        }
        else
        {
            return View();
        }
    }

    [HttpGet]
    public IActionResult Edit(long id)
    {
        var user = _userRepository.Users.FirstOrDefault(o => o.Id == id);
        var model = new UserModel
        {
            Id = id,
            Name = user.Name,
            Description = user.Description,
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(UserModel model)
    {
        var user = _userRepository.Users.FirstOrDefault(o => o.Id == model.Id);
        user.Name = model.Name;
        user.Description = model.Description;

        return RedirectToAction(nameof(Index));

    }
}


