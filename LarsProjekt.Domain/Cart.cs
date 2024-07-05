namespace LarsProjekt.Domain;

public class Cart
{
    public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    public List<Offer> Offers { get; set; } = new List<Offer>();
    public decimal Total { get; set; }
}
