using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class OrderDetailService : IOrderDetailService
{
    private readonly IApiClient _client;

    public OrderDetailService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<OrderDetail>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<OrderDetail>>("orderdetails", "getall", HttpMethod.Get);

        var list = new List<OrderDetail>();
        foreach (var detail in content)
        {
            list.Add(detail);
        }

        return list;
    }
    public async Task<List<OrderDetail>> GetListWithOrderId(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<OrderDetail>>("orderdetails", $"getwithorderid?id={id}", HttpMethod.Get);

        return content;
    }

    public async Task<OrderDetail> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<OrderDetail>("orderdetails", $"getbyid?id={id}", HttpMethod.Get);

        return content;
    }

    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("orderdetails", $"delete?id={id}", HttpMethod.Delete);

    }
    public async Task<OrderDetail> Update(OrderDetail orderDetail)
    {
        var requestContent = JsonSerializer.Serialize(orderDetail);
        var content = await _client.HttpResponseMessageAsyncPost<OrderDetail>("address", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<OrderDetail> Create(OrderDetail orderDetail)
    {
        var requestContent = JsonSerializer.Serialize(orderDetail);
        var content = await _client.HttpResponseMessageAsyncPost<OrderDetail>("address", "update", requestContent, HttpMethod.Post);

        return content;
    }
}