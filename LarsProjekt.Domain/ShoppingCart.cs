namespace LarsProjekt.Domain;

public class ShoppingCart
{
    public long Id { get; set; }
    public string ShoppingCartId { get; set; }
    public long ItemId { get; set; }
    public ShoppingCartItem ShoppingCartItem { get; set; }
    public List<ShoppingCartItem> Items { get; set; }

}
