namespace MyTemsAPI.Dto;

public record CouponDto(
    long Id,
    string Code,
    string? Discount,
    string Type ,
     DateTimeOffset ExpiryDate,
    bool Expired,
     int Count,
    int AppliedCount,
     string? Version
    );

