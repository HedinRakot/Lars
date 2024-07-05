namespace LarsProjekt.Dto.StoreApi.Mapping;

public static class CategoryMappingExtension
{
    public static Domain.StoreApi.Category ToDomain(this CategoryDto dto)
    {
        return new Domain.StoreApi.Category
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }

    public static CategoryDto ToDto(this Domain.StoreApi.Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name
            );
    }
}
