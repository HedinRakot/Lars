using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IShoppingCartItemRepository
{
    //void Add(ShoppingCart cart);
    //void Delete(ShoppingCart cart);
    //ShoppingCart Get(string id);
    List<ShoppingCartItem> GetAll();
    //void Update(ShoppingCart cart);
}