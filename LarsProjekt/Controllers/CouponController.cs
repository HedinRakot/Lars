using LarsProjekt.Database.Repositories;
using LarsProjekt.Domain;
using LarsProjekt.Models;
using LarsProjekt.Models.Mapping;
using Microsoft.AspNetCore.Mvc;
using LarsProjekt.Application;


namespace LarsProjekt.Controllers;

public class CouponController : Controller
{    
    private readonly ICouponService _couponService;
    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }
    public async Task<IActionResult> Index()
    {
        var models = new List<CouponModel>();
        var list = await _couponService.GetCoupons();
        foreach ( var coupon in list)
        {
            models.Add(coupon.ToModel());
        }

        return View(models);
    }

    [HttpGet]
    public async Task<IActionResult> CreateEdit(long id)
    {
        if (id == 0)
        {
            return View(new CouponModel());
        }
        else
        {
            var coupon = await _couponService.GetById(id);
            var model = coupon.ToModel();
            return View(model);
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
