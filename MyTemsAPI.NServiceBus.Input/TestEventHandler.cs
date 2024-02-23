using LarsProjekt.Messages;
using MyTemsAPI.Domain;
using MyTemsAPI.Domain.IRepositories;
using NServiceBus;
using System.Text.Json;

namespace MyTemsAPI.NServiceBus.Input;

public class TestEventHandler : IHandleMessages<OrderStartedEvent>
{
    //private readonly ISqlUnitOfWork _unitOfWork;
    //private readonly ICouponCountService _couponCountService;

    //public TestEventHandler(ICouponCountService couponCountService, ISqlUnitOfWork unitOfWork)
    //{
    //    _couponCountService = couponCountService;
    //    _unitOfWork = unitOfWork;
    //}

    public Task Handle(OrderStartedEvent order, IMessageHandlerContext context)
    {
        var o = JsonSerializer.Serialize(order.Order);
        var oe = JsonSerializer.Deserialize<OrderEvent>(o);

        Order domainOrder = new()
        {
            Total = oe.Total,
            Date = oe.CreatedDate,
            AddressId = oe.AddressId,
            UserId = oe.UserId,
            Details = oe.Details
        };
        //_unitOfWork.OrderRepository.Add(domainOrder);
        //_unitOfWork.SaveChanges();
        
        //foreach (var coupon in oe.Coupons)
        //{
        //    _couponCountService.UpdateCouponCount(coupon.Code);
        //}

        return Task.CompletedTask;
    }
}
public class TestCommandHandler : IHandleMessages<TestCommand>
{
    public Task Handle(TestCommand testCommand, IMessageHandlerContext context)
    {
        return Task.CompletedTask;
    }
}