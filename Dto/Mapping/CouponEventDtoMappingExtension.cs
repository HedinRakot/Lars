using LarsProjekt.Domain;
using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Dto;

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
}
