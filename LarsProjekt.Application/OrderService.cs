using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class OrderService : IOrderService
{
    private readonly IApiClient _client;

    public OrderService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Order>> Get()
    {
        var content = await _client.GetHttpResponseMessageAsync<List<OrderDto>>("order", "getall", HttpMethod.Get);

        var orders = new List<Order>();
        foreach (var order in content)
        {
            orders.Add(order.ToDomain());
        }

        return orders;
    }

    public async Task<Order> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Order>("order", $"getbyid?id={id}", HttpMethod.Get);

        return content;
    }
    public async Task<Order> Update(Order order)
    {
        var content = await _client.GetHttpResponseMessageAsync<Order>("order", "update", HttpMethod.Put);

        //var requestContent = JsonSerializer.Serialize(order.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<Order> Create(Order order)
    {
        var content = await _client.GetHttpResponseMessageAsync<Order>("order", "create", HttpMethod.Post);

        //var requestContent = JsonSerializer.Serialize(order.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Order>("products", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}