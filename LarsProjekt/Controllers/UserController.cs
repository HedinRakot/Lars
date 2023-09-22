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
                LastName = user.LastName,
                Description = user.Description,
                Email = user.Email,

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
            LastName = user.LastName,
            Description = user.Description,
            Email = user.Email,
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
    public IActionResult CreateEdit(int id)
    {
        if (id == 0)
        {
            return View(new UserModel());
        }
        else
        {
            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            var model = new UserModel
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                LastName = user.LastName,
                Description = user.Description,
            };
            return View(model);
        }

    }


    [HttpPost]
    public IActionResult CreateEdit(UserModel model)
    {
        if (model == null)
        {
            if (ModelState.IsValid)
            {
                var user = new User();
                user.Id = model.Id;
                user.Name = model.Name;
                user.LastName = model.LastName;
                user.Description = model.Description;
                user.Email = model.Email;
                var maxId = _userRepository.Users.Max(u => u.Id);
                user.Id = maxId + 1;
                _userRepository.Users.Add(user);
                return RedirectToAction(nameof(Index));
            }
            else { return View(); }
        }
        else
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.Users.FirstOrDefault(u => u.Id == model.Id);
                user.Name = model.Name;
                user.LastName = model.LastName;
                user.Description = model.Description;
                user.Email = model.Email;
                return RedirectToAction(nameof(Details), new { Id = model.Id });
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

        var model = _userRepository.Users.FirstOrDefault(p => p.Id == id);
        _userRepository.Users.Remove(model);
        return Ok(new { success = "true" });
    }
}


//    [HttpGet]
//    public IActionResult Create()
//    {
//        return View(new UserModel());
//    }

//    [HttpPost]
//    public IActionResult Create(UserModel model)
//    {
//        if (ModelState.IsValid)
//        {
//            var user = new User();
//            user.Name = model.Name;
//            user.Description = model.Description;
//            user.LastName = model.LastName;
//            user.Email = model.Email;
//            var maxId = _userRepository.Users.Max(o => o.Id);
//            user.Id = maxId + 1;

//            _userRepository.Users.Add(user);

//            return RedirectToAction(nameof(Index));
//        }
//        else
//        {
//            return View();
//        }
//    }

//    [HttpGet]
//    public IActionResult Edit(long id)
//    {
//        var user = _userRepository.Users.FirstOrDefault(o => o.Id == id);
//        var model = new UserModel
//        {
//            Id = id,
//            Name = user.Name,
//            Description = user.Description,
//            LastName = user.LastName,
//            Email = user.Email,
//        };
//        return View(model);
//    }

//    [HttpPost]
//    public IActionResult Edit(UserModel model)
//    {
//        var user = _userRepository.Users.FirstOrDefault(o => o.Id == model.Id);
//        user.Name = model.Name;
//        user.Description = model.Description;
//        user.LastName = model.LastName;
//        user.Email = model.Email;

//        return RedirectToAction(nameof(Index));

//    }
//}


