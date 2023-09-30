using LarsProjekt.Domain;

namespace LarsProjekt.Models.Mapping;

public static class ShoppingCartModelMappingExtension
{
    public static ShoppingCartModel ToModel(this ShoppingCart cart)
    {
        return new ShoppingCartModel
        {
            
            ShoppingCartId = cart.ShoppingCartId            
        };
    }
    public static ShoppingCart ToDomain(this ShoppingCartModel model)
    {
        return new ShoppingCart
        {
            
            ShoppingCartId = model.ShoppingCartId
        };
    }
}
