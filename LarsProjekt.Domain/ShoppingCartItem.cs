namespace LarsProjekt.Domain;

public class ShoppingCartItem
{
    public Product Product { get; set; }
    public long ProductId { get; set; }
    public string? Name { get; set; }
    public decimal PriceOffer { get; set; }
    public int Amount { get; set; }    
    public decimal DiscountedPrice { get; set; }
    public decimal Discount { get; set;}

}
