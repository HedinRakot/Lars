using FluentAssertions;
using LarsProjekt.Domain;
using LarsProjekt.IntegrationTests.TestSetup;
using NSubstitute;
using System.Net.Http.Json;

namespace LarsProjekt.IntegrationTests.Tests;

public class ProductControllerTest : IClassFixture<IntegrationTestsFixture>
{
    private const string RequestUri = "products";
    private readonly HttpClient _httpClient;
    private readonly IntegrationTestsFixture _fixture;

    public ProductControllerTest(IntegrationTestsFixture fixture)
    {
        _fixture = fixture;
        _httpClient = _fixture.CreateClient();

        _httpClient.DefaultRequestHeaders.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");
    }

    [Fact]
    public async Task Product_Index_Returns_Result()
    {
        //arrange
        _fixture.TestProductService.GetProducts().Returns(
            new List<Product>()
            {
                new() {
                    Id = 1,
                    Name = "Test",
                    PriceOffer = 222
                }
            });

        //act
        var response = await _httpClient.GetAsync(RequestUri + "/getall");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<Product>>();

        result.Count.Should().BeGreaterThan(0);
        result.FirstOrDefault().Name.Should().NotBeNull();
        result.FirstOrDefault().Description.Should().BeNull();
    }
}
