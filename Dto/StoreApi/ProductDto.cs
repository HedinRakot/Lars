namespace LarsProjekt.Dto.StoreApi;

public record ProductDto(
    long? Id,
    string Name,
    string Description,
    List<Domain.StoreApi.Category> Categories,
    //long CategoryId,
    string? Image
);
