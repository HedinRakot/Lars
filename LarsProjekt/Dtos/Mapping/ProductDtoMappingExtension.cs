using LarsProjekt.Domain;

namespace LarsProjekt.Dtos.Mapping;

public static class ProductDtoMappingExtension
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name,
            product.Description,
            product.Category,
            product.Price,
            product.PriceOffer,
            product.Image
            );
    }

    public static Product ToDomain(this ProductDto productDto)
    {
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Description = productDto.Description,
            Category = productDto.Category,
            Price = productDto.Price,
            PriceOffer = productDto.PriceOffer,
            Image = productDto.Image
        };
    }
}
