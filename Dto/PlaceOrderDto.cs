namespace LarsProjekt.Dto;

public class PlaceOrderDto
{
    public OrderDto Order { get; set; }
    public List<CouponDto> Coupons { get; set; } = new List<CouponDto>();

}
