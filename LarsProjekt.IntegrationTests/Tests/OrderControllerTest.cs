using FluentAssertions;
using LarsProjekt.Domain;
using LarsProjekt.IntegrationTests.TestSetup;
using LarsProjekt.Messages;
using LarsProjekt.Messages.Dtos;
using LarsProjekt.Models;
using NServiceBus;
using NServiceBus.Testing;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace LarsProjekt.IntegrationTests.Tests;

public class OrderControllerTest : IClassFixture<IntegrationTestsFixture>
{
    private const string RequestUri = "Order";
    private readonly HttpClient _httpClient;
    private readonly IntegrationTestsFixture _fixture;
    public OrderControllerTest(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
        _httpClient = _fixture.CreateClient();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");
    }

    [Fact]
    public async Task Order_Index_Returns_Result()
    {
        _fixture.TestUserService.GetByNameWithAddress(Arg.Any<string>()).Returns(
            new User
            {
                Id = 4,
                Username = "Lars"
            });
        _fixture.TestOrderService.Get().Returns(
            new List<Order>()
            {
                new Order()
                {
                    Id = 1,
                    Date = DateTime.Now,
                    AddressId = 4,
                    Total = 1066,
                    UserId = 4,
                    Details = new List<OrderDetail>()
                    {
                        new()
                        {
                            Id=1,
                            Discount=0,
                            DiscountedPrice=0,
                            OrderId=1,
                            ProductId=1,
                            Quantity=3,
                            UnitPrice=55555
                        },
                        new()
                        {
                            Id=2,
                            Discount=0,
                            DiscountedPrice=0,
                            OrderId=1,
                            ProductId=2, Quantity=3,
                            UnitPrice=1000
                        }
                    }
                }
            });

        var response = await _httpClient.GetAsync(RequestUri + "/Index");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadAsStringAsync();

        result.Should().NotBeNull();
        result.Should().Contain("1.066,00");
    }
    [Fact]
    public async Task Order_Details_Returns_Result()
    {
        _fixture.TestProductService.GetById(1).Returns(new Product
        {
            Id = 1,
            Name = "Test",
            Category = "Test"
        });
        _fixture.TestOrderService.GetDetailListWithOrderId(1).Returns(
        new List<OrderDetail>()
        {
                new()
                {
                    Id=1,
                    Discount=0,
                    DiscountedPrice=0,
                    OrderId=1,
                    ProductId=1,
                    Quantity=3,
                    UnitPrice=66,

                }
        });

        var response = await _httpClient.GetAsync(RequestUri + "/Details/" + 1);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().NotBeNull();
        result.Should().Contain("66,00");
    }

    //[Fact]
    //public async Task CreateOrder_Publish_Event()
    //{
    //    await _session.Publish(new OrderEvent()
    //    {
    //        Order = new OrderEventDto()
    //        {
    //            Id = 1,
    //            Total = 999,
    //            Coupons = new(),
    //            Details = new()
    //        }
    //    });
    //    _session.SentMessages.Should().HaveCount(1);
    //}
}
