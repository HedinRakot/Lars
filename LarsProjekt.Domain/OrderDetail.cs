namespace LarsProjekt.Domain;

public class OrderDetail
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal DiscountedPrice { get; set; }
    public decimal Discount { get; set; }
    public virtual Product Product { get; set; }
    public virtual Order Order { get; set; }
}
