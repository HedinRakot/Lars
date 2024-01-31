using LarsProjekt.Domain;

namespace LarsProjekt.Application
{
    public interface ICouponService
    {
        Task<List<Coupon>> GetCoupons();
    }
}