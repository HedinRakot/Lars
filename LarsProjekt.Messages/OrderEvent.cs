using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages;

public class OrderEvent : IEvent
{
    public OrderEventDto Order { get; set; }
}
