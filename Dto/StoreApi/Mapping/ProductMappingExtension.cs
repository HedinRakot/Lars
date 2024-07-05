namespace LarsProjekt.Dto.StoreApi.Mapping;

public static class ProductMappingExtension
{
    public static Domain.StoreApi.Product ToDomain(this ProductDto dto)
    {
        long newId;
        if (dto.Id == null)
        {
            newId = 0;
        }
        else
        {
            newId = (long)dto.Id;
        }
        return new Domain.StoreApi.Product
        {
            Id = newId,
            Name = dto.Name,
            Description = dto.Description,
            //CategoryId = dto.CategoryId,
            Image = dto.Image
        };
    }
    public static ProductDto ToDto(this Domain.StoreApi.Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Categories,
            //product.CategoryId,
            product.Image
            );
    }

}
