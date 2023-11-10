using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface ICouponRepository
{
    void Add(Coupon coupon);
    void Delete(Coupon coupon);
    Coupon Get(string code);
    Coupon GetById(long id);
    List<Coupon> GetAll();
    void Update(Coupon coupon);
}