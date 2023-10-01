namespace LarsProjekt.Domain;

public class ShoppingCartItem
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; }
    public int Amount { get; set; }          
    public string ShoppingCartId { get; set; }
}
