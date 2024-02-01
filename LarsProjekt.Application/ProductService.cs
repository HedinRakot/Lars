using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class ProductService : IProductService
{
    private readonly IApiClient _client;

    public ProductService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Product>> GetProducts()
    {
        var content = await _client.GetHttpResponseMessageAsync<List<ProductDto>>("products", "getall", HttpMethod.Get);

        var products = new List<Product>();
        foreach (var product in content)
        {
            products.Add(product.ToDomain());
        }

        return products;
    }

    public async Task<Product> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Product>("products", $"getbyid?id={id}", HttpMethod.Get);

        return content;        
    }
    public async Task<Product> Update(Product product)
    {
        var content = await _client.GetHttpResponseMessageAsync<Product>("products", "update", HttpMethod.Put);

        //var requestContent = JsonSerializer.Serialize(product.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<Product> Create(Product product)
    {
        var content = await _client.GetHttpResponseMessageAsync<Product>("products", "create", HttpMethod.Post);

        //var requestContent = JsonSerializer.Serialize(product.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Product>("products", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}