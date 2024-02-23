using LarsProjekt.Messages;
using MyTemsAPI.Application.IServices;
using NServiceBus;

namespace MyTemsAPI.NServiceBus.Input;
public class CreateOrderEventHandler : IHandleMessages<OrderStartedEvent>
{
    private readonly ICreateOrderService _orderService;
    public CreateOrderEventHandler(ICreateOrderService orderService)
    {
        _orderService = orderService;
    }
    public Task Handle(OrderStartedEvent order, IMessageHandlerContext context)
    {
        _orderService.CreateOrder(order.Order);
        return Task.CompletedTask;
    }
}