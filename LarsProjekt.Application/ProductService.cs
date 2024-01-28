using LarsProjekt.Dtos;
using LarsProjekt.Dtos.Mapping;
using LarsProjekt.Domain;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class ProductService : IProductService
{
    public async Task<List<Product>> GetProducts()
    {
        var uri = "https://localhost:7182/products/";
        var httpClient = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var productDtos = JsonSerializer.Deserialize<List<ProductDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var products = new List<Product>();
            foreach (var product in productDtos)
            {                
                products.Add(product.ToDomain());
            }

            return products;
        }

        throw new Exception();
    }
}
