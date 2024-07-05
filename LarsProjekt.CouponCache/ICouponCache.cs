using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.CouponCache;

public interface ICouponCache
{
    Task<IReadOnlyCollection<Coupon>> GetCoupons();
    Task Refresh(CancellationToken cancellationToken);
}