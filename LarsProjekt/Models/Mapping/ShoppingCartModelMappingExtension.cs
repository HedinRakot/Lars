using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class ShoppingCartModelMappingExtension
{
    public static ShoppingCartModel ToModel(this ShoppingCart cart)
    {
        return new ShoppingCartModel
        {
            //ShoppingCartItemModelId = cart.ShoppingCartItemModelId,
            //Id = cart.Id,
        };
    }
    public static ShoppingCart ToDomain(this ShoppingCartModel model)
    {
        return new ShoppingCart
        {
            //ShoppingCartItemModelId = model.ShoppingCartItemModelId,
            //ShoppingCartId = model.ShoppingCartId,
        };
    }
}
