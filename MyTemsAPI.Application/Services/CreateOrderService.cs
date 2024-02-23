using LarsProjekt.Messages.Dtos;
using MyTemsAPI.Application.IServices;
using MyTemsAPI.Domain;
using MyTemsAPI.Domain.IRepositories;

namespace MyTemsAPI.Application.Services;

internal class CreateOrderService : ICreateOrderService
{
    private readonly ISqlUnitOfWork _unitOfWork;
    private readonly ICouponCountService _couponCountService;

    public CreateOrderService(ICouponCountService couponCountService, ISqlUnitOfWork unitOfWork)
    {
        _couponCountService = couponCountService;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateOrder(OrderEventDto order)
    {
        var x = order.ToDomain();

        Order domainOrder = new()
        {
            Total = x.Total,
            Date = x.Date,
            AddressId = x.AddressId,
            UserId = x.UserId,
            Details = x.Details
        };
        _unitOfWork.OrderRepository.Add(domainOrder);
        _unitOfWork.SaveChanges();

        foreach (var coupon in order.Coupons)
        {
            _couponCountService.UpdateCouponCount(coupon.Code);
        }
    }
}
