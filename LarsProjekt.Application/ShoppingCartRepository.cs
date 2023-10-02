using LarsProjekt.Domain;

namespace LarsProjekt.Application;

public class ShoppingCartRepository
{
    public List<ShoppingCartItem> ShoppingCartItems { get; set; }
    public ShoppingCartRepository()
    {        
        ShoppingCartItems = new List<ShoppingCartItem>();
    }
}
