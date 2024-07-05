namespace LarsProjekt.Domain.OrderApi;

public class Order
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public long AddressId { get; set; }
    public DateTimeOffset OrderDate { get; set; }
    public decimal Total { get; set; }
    public List<OrderDetail> OrderDetails { get; set; } = [];
    public List<Coupon> Coupons { get; set; }
}
