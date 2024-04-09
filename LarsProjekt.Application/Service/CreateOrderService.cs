using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Messages;
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
        List<Messages.Dtos.CouponDto> couponList = new();
        List<Messages.Dtos.OrderDetailDto> detailList = new();
        
        foreach (var coupon in cart.Offers)
        {
            couponList.Add(coupon.Coupon.ToMessageDto());
        }

        foreach (var item in cart.Items)
        {
            detailList.Add(new Messages.Dtos.OrderDetailDto
            {
                ProductAmount = item.Amount,
                UnitPrice = item.PriceOffer,
                ProductId = item.ProductId
            });
        }

        Messages.Dtos.OrderDto order = new()
        {
            Total = cart.Total,
            OrderDate = DateTime.UtcNow,
            AddressId = user.AddressId,
            CustomerId = user.Id,
            Details = detailList
        };

        await _messageContext.Publish(new CreateOrderEvent()
        {
            Order = order,
            Coupons = couponList
        });
    }
}
