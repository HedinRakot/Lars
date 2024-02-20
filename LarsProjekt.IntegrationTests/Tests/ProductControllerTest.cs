using FluentAssertions;
using LarsProjekt.Domain;
using LarsProjekt.IntegrationTests.TestSetup;
using LarsProjekt.Models;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;

namespace LarsProjekt.IntegrationTests.Tests;

// Controller wird im Request aufgerufen: Uri = (Conrtollername)+(Methode) Product + Index.
// Da View returned wird, kommt Html und kein Json zurück.
// Es gibt keine DB, Objekte werden mit Fixture erstellt.

public class ProductControllerTest : IClassFixture<IntegrationTestsFixture>
{
    private const string RequestUri = "Product";
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
        var response = await _httpClient.GetAsync(RequestUri + "/Index");

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain(">Test</");
        result.Should().Contain("222");
    }
}
