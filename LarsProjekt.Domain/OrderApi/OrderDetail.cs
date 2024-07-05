namespace LarsProjekt.Domain.OrderApi;

public class OrderDetail
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int ProductAmount { get; set; }
    public decimal UnitPrice { get; set; }
    public Order Order { get; set; }
}
