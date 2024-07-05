using LarsProjekt.Domain.Interfaces;
using LarsProjekt.Domain.MyTemsApi;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Dto.MyTemsApi;
using System.Text.Json;

namespace LarsProjekt.MyTemsApiAdapter.Services;

internal class ProductService : IProductService
{
    private readonly IMyTemsApiClient _client;

    public ProductService(IMyTemsApiClient client)
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
    public async Task<Product> Update(Product product)
    {
        var requestContent = JsonSerializer.Serialize(product.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<ProductDto>("products", "update", requestContent, HttpMethod.Put);

        return content.ToDomain();
    }
    public async Task<Product> Create(Product product)
    {
        var requestContent = JsonSerializer.Serialize(product.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<ProductDto>("products", "create", requestContent, HttpMethod.Post);

        return content.ToDomain();
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("products", $"delete?id={id}", HttpMethod.Delete);
    }
}