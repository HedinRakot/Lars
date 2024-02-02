using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class OrderDetailService : IOrderDetailService
{
    private readonly IApiClient _client;

    public OrderDetailService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<OrderDetail>> Get()
    {
        var content = await _client.GetHttpResponseMessageAsync<List<OrderDetail>>("orderdetails", "getall", HttpMethod.Get);

        var list = new List<OrderDetail>();
        foreach (var detail in content)
        {
            list.Add(detail);
        }

        return list;
    }
    public async Task<List<OrderDetail>> GetListWithOrderId(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<List<OrderDetail>>("orderdetails", $"getwithorderid?id={id}", HttpMethod.Get);

        return content;
    }

    public async Task<OrderDetail> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<OrderDetail>("orderdetails", $"getbyid?id={id}", HttpMethod.Get);

        return content;
    }

    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<OrderDetail>("orderdetails", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}