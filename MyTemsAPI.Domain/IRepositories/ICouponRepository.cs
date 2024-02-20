

namespace MyTemsAPI.Domain.IRepositories
{
    public interface ICouponRepository
    {
        void Add(Coupon coupon);
        void Delete(Coupon coupon);
        List<Coupon> GetAll();
        Coupon GetById(long id);
        Coupon GetByName(string name);
        void Update(Coupon coupon);
        Coupon UpdateCouponCount(Coupon coupon);
    }
}