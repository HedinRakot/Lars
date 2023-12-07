namespace LarsProjekt.Models;

public class OrderDetailModel
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public virtual ShoppingCartItemModel ItemModel { get; set; }    
    
    //public virtual OrderModel Order { get; set; }
}