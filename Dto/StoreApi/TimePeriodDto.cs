namespace LarsProjekt.Dto.StoreApi;

public record TimePeriodDto(
    long Id,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    List<ProductDto> Products
    );
