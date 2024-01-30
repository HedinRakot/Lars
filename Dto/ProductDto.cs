namespace LarsProjekt.Dto;

public record ProductDto(
    long Id,
    string Name,
    string? Description,
    string? Category,
    decimal Price,
    decimal PriceOffer,
    string? Image
);

