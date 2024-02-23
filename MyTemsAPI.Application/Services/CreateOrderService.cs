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

    public async Task CreateOrder(OrderEvent order)
    {
        Order domainOrder = new()
        {
            Total = order.Total,
            Date = order.CreatedDate,
            AddressId = order.AddressId,
            UserId = order.UserId,
            Details = order.Details
        };
        _unitOfWork.OrderRepository.Add(domainOrder);
        await _unitOfWork.SaveChangesAsync();

        foreach (var coupon in order.Coupons)
        {
            _couponCountService.UpdateCouponCount(coupon.Code);
        }
    }
}
