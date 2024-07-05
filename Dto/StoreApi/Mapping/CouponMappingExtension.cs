using System.Text;

namespace LarsProjekt.Dto.StoreApi.Mapping;

public static class CouponMappingExtension
{
    public static Domain.StoreApi.Coupon ToDomain(this CouponDto dto)
    {
        return new Domain.StoreApi.Coupon
        {
            Id = dto.Id,
            Code = dto.Code,
            Discount = dto.Discount,
            Count = dto.Count,
            AppliedCount = dto.AppliedCount,
            Expired = dto.Expired,
            ExpiryDate = dto.ExpiryDate,
            Type = dto.Type,
            Version = dto.Version != null ? Encoding.UTF8.GetBytes(dto.Version) : null
        };
    }

    public static CouponDto ToDto(this Domain.StoreApi.Coupon coupon)
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
            Encoding.UTF8.GetString(coupon.Version)
            );
    }
}
