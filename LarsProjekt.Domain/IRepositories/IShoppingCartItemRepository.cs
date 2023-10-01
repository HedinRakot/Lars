using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IShoppingCartItemRepository
{
    List<ShoppingCartItem> GetAll();
    //ShoppingCartItem AddToCart(Product product);
    ShoppingCartItem GetItem(int id);
    ShoppingCartItem AddProduct(ShoppingCartItem product);

}