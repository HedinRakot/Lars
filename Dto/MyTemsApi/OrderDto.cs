using LarsProjekt.Domain;

namespace LarsProjekt.Dto.MyTemsApi;

public record OrderDto(
    long Id,
    decimal? Total,
    DateTimeOffset Date,
    long UserId,
    long AddressId,
    List<OrderDetailDto> Details
    );


