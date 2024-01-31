using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class CouponDtoMappingExtension
{
    public static CouponDto ToDto(this Coupon coupon)
    {
        return new CouponDto(
            coupon.Id,
            coupon.Code,
            coupon.Discount,
            coupon.Type,
            coupon.ExpiryDate,
            coupon.Expired,
            coupon.Count,
            coupon.AppliedCount,
            coupon.Version
            );
    }
    public static Coupon ToDomain(this CouponDto dto)
    {
        return new Coupon
        {
            Id = dto.Id,
            Code = dto.Code,
            Discount = dto.Discount,
            Count = dto.Count,
            AppliedCount = dto.AppliedCount,
            Expired = dto.Expired,
            ExpiryDate = dto.ExpiryDate,
            Type = dto.Type
        };
    }
}
