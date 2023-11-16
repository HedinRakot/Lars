using LarsProjekt.Database;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using LarsProjekt.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers;
public class UserController : Controller
{
    private readonly IAddressRepository _addressRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISqlUnitOfWork _unitOfWork;
    public UserController(IUserRepository userRepository, IAddressRepository addressRepository, ISqlUnitOfWork sqlUnitOfWork)
    {
        _userRepository = userRepository;
        _addressRepository = addressRepository;
        _unitOfWork = sqlUnitOfWork;
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
    public IActionResult CreateEdit()
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);

        if( signedInUser != null )
        {
            var address = _unitOfWork.AddressRepository.Get(signedInUser.AddressId);
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
    public IActionResult CreateEdit(UserRegistrationVM model)
    {
        var signedInUser = _userRepository.GetByName(HttpContext.User.Identity.Name);

        if (ModelState.IsValid)
        {
            if (signedInUser == null)
            {
                var user = model.UserModel.ToDomain();
                var address = model.AddressModel.ToDomain();
                user.Address = address;
                _unitOfWork.AddressRepository.Add(address);               
                _unitOfWork.UserRepository.Add(user);                
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(CreateEdit));

                // TODO Login nach dem registrieren
                //
                //var logInModel = new LoginModel()
                //{
                //    UserName = model.UserModel.Username,
                //    Password = model.UserModel.Password
                //};
                //return RedirectToAction("SignIn", "LoginController", logInModel);
            }
                // TODO Nutzernamen ändern
            if (model.UserModel.Id == signedInUser.Id)
            {
                //var user = model.UserModel.ToDomain();
                var address = model.AddressModel.ToDomain();
                //_unitOfWork.UserRepository.Update(user);
                _unitOfWork.AddressRepository.Update(address);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(CreateEdit));
            }
        } else
        {
            ModelState.AddModelError("Model", "Please check your information");
        }
        return View(model);
    }
}

