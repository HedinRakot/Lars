using LarsProjekt.Domain;

namespace LarsProjekt.Models;

public class ShoppingCartItemModel
{
    public int ShoppingCartItemModelId { get; set; } = 0;
    public Product Product { get; set; }
    public int Amount { get; set; }
    public string ShoppingCartId { get; set; }

}
