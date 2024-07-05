namespace LarsProjekt.Dto.OrderApi;

public record OrderDetailDto(
    long Id,
    long OrderId,
    long ProductId,
    int ProductAmount,
    decimal UnitPrice
    );


