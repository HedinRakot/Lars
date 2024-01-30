using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Domain;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class ProductService : IProductService
{
    public async Task<List<Product>> GetProducts()
    {
        var uri = "https://localhost:7182/api/products/getall";
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

    public async Task<Product> GetById(long id)
    {
        var uri = $"https://localhost:7182/api/products/getbyid{id}";
        var httpClient = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var productDto = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return productDto.ToDomain();
        }

        throw new Exception();
    }
    public async Task<Product> Update(ProductDto productDto)
    {
        var uri = $"https://localhost:7182/api/products/update";
        var httpClient = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???
        
        var requestContent = JsonSerializer.Serialize(productDto);
        httpRequestMessage.Content = new StringContent(requestContent);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result.ToDomain();
        }

        throw new Exception();
    }
    public async Task<Product> Create(Product product)
    {
        var uri = $"https://localhost:7182/api/products/create";
        var httpClient = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???

        var requestContent = JsonSerializer.Serialize(product.ToDto());
        httpRequestMessage.Content = new StringContent(requestContent);

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result.ToDomain();
        }

        throw new Exception();
    }
    public async void Delete(long id)
    {
        var uri = $"https://localhost:7182/api/products/delete{id}";
        var httpClient = new HttpClient();
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???

        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            throw new Exception();
        }

        
    }
}
