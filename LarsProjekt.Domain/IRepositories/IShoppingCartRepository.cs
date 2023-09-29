using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

public interface IShoppingCartRepository
{
    void Add(ShoppingCart cart);
    void Delete(ShoppingCart cart);
    ShoppingCart Get(string id);
    List<ShoppingCart> GetAll();
    void Update(ShoppingCart cart);
}