using LarsProjekt.Application;
using LarsProjekt.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LarsProjekt.Controllers
{
    public class HomeController : Controller
    {
        private Singleton _singleton;
        private Scoped _scoped;
        private ScopedDependent _scopedDependent;

        public HomeController(Singleton singleton, Scoped scoped, ScopedDependent scopedDependent)
        {
            _singleton = singleton;
            _scoped = scoped;
            _scopedDependent = scopedDependent;
        }

        public IActionResult Index()
        //Method injection public IActionResult Index([FromServices]Singleton singleton)
        {
            //_singleton.Count++;

            _singleton.CalledAt.Add(DateTime.Now);
            _singleton.CalledAtWithOffset.Add(DateTimeOffset.Now);

            _singleton.CalledAtAsString.Add(DateTime.Now.ToString());

            //_singleton.CalledAt.Count

            //DateTime als string auf einer englischer Maschine "09/08/2023 20:49:00"


            _scoped.Calls.Add(DateTimeOffset.Now);

            var calculated = _scopedDependent.Calculate();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}