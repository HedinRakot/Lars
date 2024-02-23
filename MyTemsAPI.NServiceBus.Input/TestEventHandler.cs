using LarsProjekt.Messages;
using MyTemsAPI.Application.IServices;
using MyTemsAPI.Domain;
using NServiceBus;
using System.Text.Json;

namespace MyTemsAPI.NServiceBus.Input;
public class TestEventHandler : IHandleMessages<OrderStartedEvent>
{
    private readonly ICreateOrderService _orderService;
    public TestEventHandler(ICreateOrderService orderService)
    {
        _orderService = orderService;
    }
    public Task Handle(OrderStartedEvent order, IMessageHandlerContext context)
    {        
        var o = JsonSerializer.Serialize(order.Order);
        var oe = JsonSerializer.Deserialize<OrderEvent>(o);

        _orderService.CreateOrder(oe);

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