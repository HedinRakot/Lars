using FluentAssertions;
using LarsProjekt.Domain;
using LarsProjekt.IntegrationTests.TestSetup;
using LarsProjekt.Models;
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
        _fixture.TestUserService.GetByName("Lars").Returns(
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
                            UnitPrice=66
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

        var result = await response.Content.ReadFromJsonAsync<List<Order>>();

        result.Should().NotBeNull();
        result.Count.Should().Be(1);
        result.FirstOrDefault().Details.Count.Should().Be(2);
        result.FirstOrDefault().Total.Should().Be(1066);
    }
    [Fact]
    public async Task Order_Details_Returns_Result()
    {
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
                    UnitPrice=66                    
                },
                new()
                {
                    Id=2,
                    Discount=0,
                    DiscountedPrice=0,
                    OrderId=1,
                    ProductId=2,
                    Quantity=9,
                    UnitPrice=1000
                }
            });

        var response = await _httpClient.GetAsync(RequestUri + "/Details" + 1);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<OrderDetailModel>>();
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result.FirstOrDefault(x => x.Id == 2).Quantity.Should().Be(9);
        result.FirstOrDefault().Product.Should().BeNull();
    }
}
