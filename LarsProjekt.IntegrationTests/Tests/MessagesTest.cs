using FluentAssertions;
using LarsProjekt.IntegrationTests.TestSetup;
using LarsProjekt.Messages;
using LarsProjekt.Messages.Dtos;
using NServiceBus;
using NServiceBus.Testing;

namespace LarsProjekt.IntegrationTests.Tests;

public class MessagesTest : IClassFixture<IntegrationTestsFixture>
{
    private readonly IntegrationTestsFixture _fixture;

    public MessagesTest(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task CreateOrder_Publish_Event_Should_Send_Message_Correctly()
    {
        await _fixture.MyMessageTestSession.Publish(new CreateOrderEvent()
        {
            Order = new OrderDto()
            {
                Id = 1,
                Total = 999,
                Coupons = new(),
                Details = new()
            }
        }, new PublishOptions { }
        );

        _fixture.MyMessageTestSession.PublishedMessages.Should().HaveCount(1);
    }
}
