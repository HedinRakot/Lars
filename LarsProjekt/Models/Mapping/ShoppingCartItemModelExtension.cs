using LarsProjekt.Domain;
using System.Runtime.CompilerServices;

namespace LarsProjekt.Models.Mapping;

public static class ShoppingCartItemModelExtension
{
    public static ShoppingCartItemModel ToModel(this ShoppingCartItem item)
    {
        return new ShoppingCartItemModel
        {
            ShoppingCartItemModelId = item.ShoppingCartItemModelId,
            ShoppingCartId = item.ShoppingCartId,
            Amount = item.Amount,
        };
    }
    public static ShoppingCartItem ToDomain(this ShoppingCartItemModel model)
    {
        return new ShoppingCartItem
        {
            ShoppingCartItemModelId = model.ShoppingCartItemModelId,
            ShoppingCartId = model.ShoppingCartId,
            Amount = model.Amount,
        };
    }
}
