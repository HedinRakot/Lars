namespace LarsProjekt.Dto.OrderApi;

public record OrderDto(
    long Id,
    decimal Total,
    long AddressId,
    long CustomerId,
    DateTimeOffset Date,
    List<OrderDetailDto> Details
    );


