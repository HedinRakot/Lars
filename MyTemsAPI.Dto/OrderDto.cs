

using MyTemsAPI.Domain;

namespace MyTemsAPI.Dto;

public record OrderDto(
    long Id ,
    decimal? Total ,
    long AddressId,
    long UserId,
    DateTimeOffset Date,
    List<OrderDetailDto> Details
    );


