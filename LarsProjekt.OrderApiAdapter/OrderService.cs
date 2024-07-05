using LarsProjekt.Domain.Interfaces;
using LarsProjekt.Domain.MyTemsApi;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Dto.MyTemsApi;
using System.Text.Json;

namespace LarsProjekt.OrderApiAdapter;

internal class OrderService : IOrderService
{
    private readonly IOrderApiClient _client;

    public OrderService(IOrderApiClient client)
    {
        _client = client;
    }
    public async Task<List<Order>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<OrderDto>>("order", "getall", HttpMethod.Get);

        var orders = new List<Order>();
        foreach (var order in content)
        {
            orders.Add(order.ToDomain());
        }
        return orders;
    }
    public async Task<List<OrderDetail>> GetDetailListWithOrderId(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<OrderDetailDto>>("orderdetail", $"getwithorderid?id={id}", HttpMethod.Get);

        var details = new List<OrderDetail>();
        foreach (var detail in content)
        {
            details.Add(detail.ToDomain());
        }
        return details;
    }

    public async Task<Order> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<OrderDto>("order", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<Order> Update(Order order)
    {
        var requestContent = JsonSerializer.Serialize(order.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Order>("order", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("order", $"delete?id={id}", HttpMethod.Delete);
    }
}