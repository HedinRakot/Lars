using FluentAssertions;
using MyTemsAPI.Domain;
using MyTemsAPI.Dto;
using MyTemsAPI.IntegrationTests.TestSetup;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace MyTemsAPI.IntegrationTests.Tests;

// echten dbcontext 
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
    public async Task GetAll_WithoutAnyProducts_ReturnsEmptyResponse()
    {
        // arrange

        // act
        var response = await _httpClient.GetAsync(RequestUri + "/getall");

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.Content.ReadFromJsonAsync<List<Product>>()).Should().BeEmpty();
    }

    [Fact]
    public async Task Add_Product_Returns_Result()
    {
        // arrange
        var newProduct = new Product
        {
            Name = "Test",
            Description = "TestDescription",
            Price = 10,
            PriceOffer = 9,
            Category = "TestCategory",
            Image = null
        };

        // act
        var response = await _httpClient.PostAsync(RequestUri + "/create", new StringContent(JsonSerializer.Serialize(newProduct), Encoding.UTF8, "application/json"));

        // assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<ProductDto>();

        var dbProduct = _fixture.GetProduct(result.Id);
        dbProduct.Should().NotBeNull();
        
        result.Id.Should().Be(dbProduct.Id);
        result.Name.Should().Be("Test");
        result.Image.Should().BeNull();
    }
}
