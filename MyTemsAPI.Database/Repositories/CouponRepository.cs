using MyTemsAPI.Database;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Domain;
using MyTemsAPI.Database.Concurrency;

namespace MyTemsAPI.Repositories;

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context;
    public CouponRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Coupon> GetAll()
    {
        return _context.Coupons.ToList();
    }

    public Coupon GetById(long id)
    {
        return _context.Coupons.FirstOrDefault(x => x.Id == id);
    }
    public Coupon GetByName(string name)
    {
        var coupon = _context.Coupons.FirstOrDefault(x => x.Code == name);
        if (coupon == null)
        {
            return new Coupon();
        }
        return coupon;
    }

    public void Add(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        _context.SaveChanges();
    }
    public void Update(Coupon coupon)
    {
        _context.Coupons.Update(coupon);
        _context.SaveChangesAsyncWithConcurrencyCheckAsync();
    }
    public void Delete(Coupon coupon)
    {
        _context.Coupons.Remove(coupon);
        _context.SaveChanges();
    }
    public Coupon UpdateCouponCount(Coupon coupon)
    {
        coupon.AppliedCount++;
        _context.Update(coupon);
        return coupon;
    }
}
