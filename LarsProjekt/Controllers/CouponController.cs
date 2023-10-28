using LarsProjekt.Application;
using LarsProjekt.Database.Repositories;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace LarsProjekt.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        public IActionResult Index()
        {
            var list = new List<CouponModel>();
            foreach ( var coupon in _couponRepository.GetAll())
            {
                list.Add(coupon.ToModel());
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult CreateEdit(long id)
        {
            if (id == 0)
            {
                return View(new CouponModel());
            }
            else
            {
                var coupon = _couponRepository.Get(id);
                var model = coupon.ToModel();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult CreateEdit(CouponModel model)
        {

            if (model.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    var coupon = model.ToDomain();
                    _couponRepository.Add(coupon);
                    return RedirectToAction(nameof(Index));
                }
                else { return View(); }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var coupon = model.ToDomain();
                    _couponRepository.Update(coupon);
                    return RedirectToAction(nameof(Index), new { Id = coupon.Id });
                }
                else return View();
            }
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            try
            {
                var model = _couponRepository.Get(id);
                _couponRepository.Delete(model);
                return Ok(new { success = "true" });
            }
            catch
            {
                throw new BadHttpRequestException("Oops, please try again!", 400);
            }
        }
    }
}
