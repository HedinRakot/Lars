namespace LarsProjekt.Dtos;

public record ProductDto(
    long Id,
    string Name,
    string? Description,
    string? Category,
    decimal Price,
    decimal PriceOffer,
    string? Image
);

//public record CreateProductDto(
//    string Name,
//    string Description,
//    string Category,
//    decimal Price,
//    decimal PriceOffer,
//    string Image
//);
