using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages;

public class CreateOrderEvent : IEvent
{
    public OrderDto Order {  get; set; }
    public List<CouponDto> Coupons { get; set; }
}
