using LarsProjekt.Domain;

namespace LarsProjekt.Application
{
    public interface ICouponService
    {
        Task<List<Coupon>> GetCoupons();
        Task<Coupon> GetByName(string name);
        Task<Coupon> GetById(long id);
        Task<Coupon> Update(Coupon coupon);
        Task<Coupon> Create(Coupon coupon);
        Task<string> Delete(long id);
    }
}