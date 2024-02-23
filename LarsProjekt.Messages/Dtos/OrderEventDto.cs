namespace LarsProjekt.Messages.Dtos;

public class OrderEventDto
{
    public long Id { get; set; }
    public decimal? Total { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public long UserId { get; set; }
    public long AddressId { get; set; }
    public List<OrderDetailEventDto> Details { get; set; }
    public List<CouponEventDto> Coupons { get; set; }
}

