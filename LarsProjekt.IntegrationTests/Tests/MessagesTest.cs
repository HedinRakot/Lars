using FluentAssertions;
using LarsProjekt.Messages;
using LarsProjekt.Messages.Dtos;
using NServiceBus;
using NServiceBus.Testing;

namespace LarsProjekt.IntegrationTests.Tests;

public class MessagesTest
{
    private readonly TestableMessageSession _session;
    public MessagesTest(TestableMessageSession session)
    {
        _session = session;
    }

    [Fact]
    public async Task CreateOrder_Publish_Event_Should_Send_Message_Correctly()
    {
        await _session.Publish(new OrderEvent()
        {
            Order = new OrderEventDto()
            {
                Id = 1,
                Total = 999,
                Coupons = new(),
                Details = new()
            }
        }, new PublishOptions { }
        );

        _session.SentMessages.Should().HaveCount(1);
    }
}
