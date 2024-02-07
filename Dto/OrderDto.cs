using LarsProjekt.Domain;

namespace LarsProjekt.Dto;

public record OrderDto(
    long Id ,
    decimal? Total ,
    DateTimeOffset Date,
    long UserId,
    long AddressId,
    List<OrderDetail> Details
    );


