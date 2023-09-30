using LarsProjekt.Domain;

namespace LarsProjekt.Models;

public class ShoppingCartItemModel
{
    public long ItemId { get; set; }
    public ProductModel Product { get; set; }
    public int Amount { get; set; }
    public string ShoppingCartId { get; set; }

}
