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
                var coupon = _couponRepository.GetById(id);
                var model = coupon.ToModel();
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult CreateEdit(CouponModel model)
        {
            if(ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    var coupon = model.ToDomain();
                    _couponRepository.Add(coupon);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var coupon = model.ToDomain();
                    _couponRepository.Update(coupon);
                    return RedirectToAction(nameof(Index), new { Id = coupon.Id });
                }
            }
            return View(model);            
        }

        [HttpDelete]
        public IActionResult Delete(long id)
        {
            try
            {
                var model = _couponRepository.GetById(id);
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
