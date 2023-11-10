using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _context;

    public CouponRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Coupon> GetAll()
    {
        var coupons = _context.Coupons;
        return coupons.ToList();
    }

    public void Add(Coupon coupon)
    {
        _context.Coupons.Add(coupon);
        _context.SaveChanges();
    }

    public Coupon Get(string code)
    {
        return _context.Coupons.FirstOrDefault(u => u.Code == code);
    }

    public Coupon GetById(long id)
    {
        return _context.Coupons.FirstOrDefault(u => u.Id == id);
    }

    public void Update(Coupon coupon)
    {
        _context.Coupons.Update(coupon);
        _context.SaveChanges();
    }

    public void Delete(Coupon coupon)
    {
        _context.Coupons.Remove(coupon);
        _context.SaveChanges();
    }

}
