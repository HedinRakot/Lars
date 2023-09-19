using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
