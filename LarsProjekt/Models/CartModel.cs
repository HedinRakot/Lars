namespace LarsProjekt.Models;

public class CartModel
{    
    public List<ShoppingCartItemModel> Items { get; set; } = new List<ShoppingCartItemModel>();
    public List<OfferModel> Offers { get; set; } = new List<OfferModel>();
    public decimal Total { get; set; }
    public decimal Discount { get; set; }
    
}
