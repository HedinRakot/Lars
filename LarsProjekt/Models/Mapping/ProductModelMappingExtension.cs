using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class ProductModelMappingExtension
{
    public static ProductModel ToModel(this Product product)
    {
        return new ProductModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            Picture = product.Picture,
            PriceOffer = product.PriceOffer,
        };
    }
    public static Product ToDomain(this ProductModel model)
    {
        return new Product
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Category = model.Category,
            Price = model.Price,
            PriceOffer = model.PriceOffer,
            
        };
    }

}


