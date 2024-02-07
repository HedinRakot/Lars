using LarsProjekt.Domain;

namespace LarsProjekt.Application.IService
{
    public interface ICouponService
    {
        Task<List<Coupon>> GetCoupons();
        Task<Coupon> GetByName(string name);
        Task<Coupon> GetById(long id);
        Task<Coupon> Update(Coupon coupon);
        Task<Coupon> Create(Coupon coupon);
        Task Delete(long id);
    }
}