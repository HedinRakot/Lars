using LarsProjekt.Domain.MyTemsApi;

namespace LarsProjekt.Domain;

public class ShoppingCartItem
{
    public Product Product { get; set; }
    public long ProductId { get; set; }
    public decimal PriceOffer { get; set; }
    public int Amount { get; set; }    
}
