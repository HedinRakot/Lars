using LarsProjekt.Domain;

namespace LarsProjekt.CouponCache;

public interface ICouponCache
{
    Task<IReadOnlyCollection<Coupon>> GetCoupons();
    Task Refresh(CancellationToken cancellationToken);
}