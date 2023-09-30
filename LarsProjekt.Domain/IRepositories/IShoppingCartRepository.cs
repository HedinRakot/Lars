using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IShoppingCartRepository
{
    void AddToCart(Product product, int Amount);
    List<ShoppingCartItem> GetItems();
    int RemoveFromCart(Product product);
}