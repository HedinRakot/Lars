using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages;

public class OrderStartedEvent : IEvent
{
    public OrderEventDto Order { get; set; }
}
