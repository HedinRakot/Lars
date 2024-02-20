using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class ProductService : IProductService
{
    private readonly IApiClient _client;

    public ProductService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Product>> GetProducts()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<ProductDto>>("products", "getall", HttpMethod.Get);

        var products = new List<Product>();
        foreach (var product in content)
        {
            products.Add(product.ToDomain());
        }

        return products;
    }

    public async Task<Product> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<ProductDto>("products", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<ProductDto> Update(Product product)
    {
        var requestContent = JsonSerializer.Serialize(product.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<ProductDto>("products", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<ProductDto> Create(Product product)
    {
        var requestContent = JsonSerializer.Serialize(product.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<ProductDto>("products", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("products", $"delete?id={id}", HttpMethod.Delete);
    }
}