using LarsProjekt.Domain;
using LarsProjekt.Dto;
using System.Security.Cryptography.Xml;

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
                Count = coupon.Count,
                AppliedCount = coupon.AppliedCount,
                Version = coupon.Version
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
                Count = model.Count,
                AppliedCount = model.AppliedCount,
                Version = model.Version
                
            };
        }

        public static CouponDto ToDto(this CouponModel model)
        {
            return new CouponDto(
                model.Id,
                model.Code,
                model.Discount,
                model.Type,
                model.ExpiryDate,
                model.Expired,
                model.Count,
                model.AppliedCount,
                model.Version
                );
        }
    }
}
