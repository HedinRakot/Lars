using LarsProjekt.Messages;
using NServiceBus;

namespace MyTemsAPI.NServiceBus.Input;

public class TestEventHandler : IHandleMessages<TestEvent>
{
    public Task Handle(TestEvent testEvent, IMessageHandlerContext context)
    {
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