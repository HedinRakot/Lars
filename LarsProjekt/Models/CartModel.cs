namespace LarsProjekt.Models;

public class CartModel
{
    public string ShoppingCartId { get; set; }
    public List<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();
}
