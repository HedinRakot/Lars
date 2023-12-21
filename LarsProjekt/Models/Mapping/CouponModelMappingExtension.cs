using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping
{
    public static class CouponModelMappingExtension
    {
        public static CouponModel ToModel(this Coupon coupon)
        {
            return new CouponModel
            {
                Id = coupon.Id,
                Code = coupon.Code,
                Discount = coupon.Discount,
                Type = coupon.Type,
                ExpiryDate = coupon.ExpiryDate,
                Expired = coupon.Expired,
                Count = coupon.Count
            };
        }

        public static Coupon ToDomain(this CouponModel model)
        {
            return new Coupon
            {
                Id = model.Id,
                Code = model.Code,
                Discount = model.Discount,
                Type = model.Type,
                ExpiryDate = model.ExpiryDate,
                Count = model.Count
            };
        }
    }
}
