using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Messages;
using LarsProjekt.Messages.Dtos;
using NServiceBus;

namespace LarsProjekt.Application.Service;

internal class CreateOrderService : ICreateOrderService
{
    private readonly IMessageSession _messageContext;
    public CreateOrderService(IMessageSession messageContext)
    {
        _messageContext = messageContext;
    }
    public async Task CreateOrder(User user, Cart cart)
    {
        OrderEventDto orderEvent = new()
        {
            Total = cart.Total,
            CreatedDate = DateTime.UtcNow,
            AddressId = user.AddressId,
            UserId = user.Id
        };

        List<CouponEventDto> couponList = new();
        List<OrderDetailEventDto> detailList = new();

        foreach (var coupon in cart.Offers)
        {
            couponList.Add(coupon.Coupon.ToEventDto());
        }

        foreach (var item in cart.Items)
        {
            detailList.Add(new OrderDetailEventDto
            {
                Quantity = item.Amount,
                UnitPrice = item.PriceOffer,
                ProductId = item.ProductId,
                Discount = item.Discount,
                DiscountedPrice = item.DiscountedPrice
            });
        }
        orderEvent.Details = detailList;
        orderEvent.Coupons = couponList;

        await _messageContext.Publish(new OrderEvent()
        {
            Order = orderEvent
        });
    }
}