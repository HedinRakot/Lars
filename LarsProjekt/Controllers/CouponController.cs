using LarsProjekt.Application.IService;
using LarsProjekt.CouponCache;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;


namespace LarsProjekt.Controllers;

public class CouponController : Controller
{    
    private readonly ICouponService _couponService;
    private readonly ICouponCache _couponCache;
    public CouponController(ICouponService couponService, ICouponCache couponCache)
    {
        _couponService = couponService;
        _couponCache = couponCache;
    }
    public async Task<IActionResult> Index()
    {
        var models = new List<CouponModel>();
        var list = await _couponCache.GetCoupons();
        foreach ( var coupon in list)
        {
            models.Add(coupon.ToModel());
        }
        return View(models);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(long id)
    {
        if (id != 0)
        {
            var coupon = await _couponService.GetById(id);
            var model = coupon.ToModel();
            return View(model);
        }
        else
        {
            return View(new CouponModel());
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateEdit(CouponModel model)
    {
        if(ModelState.IsValid)
        {
            if (model.Id == 0)
            {
                var coupon = model.ToDomain();
                await _couponService.Create(coupon);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var coupon = model.ToDomain();
                await _couponService.Update(coupon);
                return RedirectToAction(nameof(Index), new { Id = coupon.Id });
            }
        }
        return View(model);            
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long id)
    {
            await _couponService.Delete(id);
            return Ok(new { success = "true" });
    }   
}
