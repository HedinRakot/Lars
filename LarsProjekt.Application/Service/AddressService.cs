using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class AddressService : IAddressService
{
    private readonly IApiClient _client;

    public AddressService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Address>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<AddressDto>>("address", "getall", HttpMethod.Get);

        var addresses = new List<Address>();
        foreach (var address in content)
        {
            addresses.Add(address.ToDomain());
        }

        return addresses;
    }

    public async Task<Address> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<AddressDto>("address", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<Address> Update(Address address)
    {
        var requestContent = JsonSerializer.Serialize(address.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Address>("address", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<Address> Create(Address address)
    {
        var requestContent = JsonSerializer.Serialize(address.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Address>("address", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("address", $"delete?id={id}", HttpMethod.Delete);        
    }
}