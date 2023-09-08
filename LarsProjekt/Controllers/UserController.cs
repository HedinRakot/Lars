using LarsProjekt.Application;
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
        foreach(var user in _userRepository.Users)
        {
            list.Add(new UserModel
            {
                Id = user.Id,
                Name = user.Name,
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
            Password = user.Password
        };

        return View(model);
    }
}
