using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class ShoppingCartModelMappingExtension
{
    public static ShoppingCartModel ToModel(this ShoppingCart cart)
    {
        return new ShoppingCartModel
        {
            ItemId = cart.ItemId,
            Id = cart.Id,
            ShoppingCartId = cart.ShoppingCartId            
        };
    }
    public static ShoppingCart ToDomain(this ShoppingCartModel model)
    {
        return new ShoppingCart
        {
            ItemId = model.ItemId,
            Id = model.Id,
            ShoppingCartId = model.ShoppingCartId
        };
    }
}
