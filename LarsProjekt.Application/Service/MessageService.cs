using LarsProjekt.Application.IService;
using LarsProjekt.Domain.Exceptions;
using LarsProjekt.Messages;
using NServiceBus;

namespace LarsProjekt.Application.Service;

internal class MessageService : IMessageService
{
    private readonly NServiceBus.IMessageSession _messageContext;

    public MessageService(NServiceBus.IMessageSession context)
    {
        _messageContext = context;
    }

    public async Task SendOrder()
    {
        try
        {
            var count = 100;
            await _messageContext.Publish(new TestEvent
            {
                Count = count
            });
            await _messageContext.Send(new TestCommand
            {
                Count = count
            });
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred: {ex}");
        }
    }
}
