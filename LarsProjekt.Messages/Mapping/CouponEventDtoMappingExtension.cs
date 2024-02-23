using LarsProjekt.Domain;
using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages.Mapping;

public static class CouponEventDtoMappingExtension
{
    public static CouponEventDto ToEventDto(this Coupon coupon)
    {
        return new CouponEventDto
        {
            Id = coupon.Id,
            Code = coupon.Code,
            Discount = coupon.Discount,
            Count = coupon.Count,
            AppliedCount = coupon.AppliedCount,
            Expired = coupon.Expired,
            ExpiryDate = coupon.ExpiryDate,
            Type = coupon.Type,
            Version = coupon.Version
        };
    }
    public static Coupon ToDomain(this CouponEventDto dto)
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
            Type = dto.Type,
            Version = dto.Version
        };
    }
}
