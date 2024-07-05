namespace LarsProjekt.Domain.Interfaces
{
    public interface ICouponService
    {
        Task<List<MyTemsApi.Coupon>> GetCoupons();
        Task<MyTemsApi.Coupon> GetByName(string name);
        Task<MyTemsApi.Coupon> GetById(long id);
        Task<MyTemsApi.Coupon> Update(MyTemsApi.Coupon coupon);
        Task<MyTemsApi.Coupon> Create(MyTemsApi.Coupon coupon);
        Task Delete(long id);
    }
}