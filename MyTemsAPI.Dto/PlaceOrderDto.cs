namespace MyTemsAPI.Dto;

public class PlaceOrderDto
{
    public OrderDto Order { get; set; }
    public List<CouponDto>? Coupons { get; set; }

}
