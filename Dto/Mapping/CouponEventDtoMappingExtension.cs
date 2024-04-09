using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class CouponEventDtoMappingExtension
{
    public static Messages.Dtos.CouponDto ToMessageDto(this Coupon coupon)
    {
        return new Messages.Dtos.CouponDto
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
