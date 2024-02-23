namespace MyTemsAPI.Domain;
public class OrderEvent
{
    public long Id { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public List<OrderDetail> Details { get; set; }
    public List<Coupon> Coupons { get; set; }
}

