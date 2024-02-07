using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class OrderService : IOrderService
{
    private readonly IApiClient _client;

    public OrderService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Order>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<OrderDto>>("orders", "getall", HttpMethod.Get);

        var orders = new List<Order>();
        foreach (var order in content)
        {
            orders.Add(order.ToDomain());
        }

        return orders;
    }

    public async Task<Order> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<OrderDto>("orders", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<Order> Update(Order order)
    {
        var requestContent = JsonSerializer.Serialize(order.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Order>("orders", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<Order> Create(Order order)
    {
        var requestContent = JsonSerializer.Serialize(order.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Order>("orders", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("orders", $"delete?id={id}", HttpMethod.Delete);
    }
}