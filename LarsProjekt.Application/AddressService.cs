using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class AddressService : IAddressService
{
    private readonly IApiClient _client;

    public AddressService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Address>> Get()
    {
        var content = await _client.GetHttpResponseMessageAsync<List<AddressDto>>("products", "getall", HttpMethod.Get);

        var addresses = new List<Address>();
        foreach (var address in content)
        {
            addresses.Add(address.ToDomain());
        }

        return addresses;
    }

    public async Task<Address> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Address>("address", $"getbyid?id={id}", HttpMethod.Get);

        return content;
    }
    public async Task<Address> Update(Address address)
    {
        var content = await _client.GetHttpResponseMessageAsync<Address>("address", "update", HttpMethod.Put);

        //var requestContent = JsonSerializer.Serialize(address.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<Address> Create(Address address)
    {
        var content = await _client.GetHttpResponseMessageAsync<Address>("address", "create", HttpMethod.Post);

        //var requestContent = JsonSerializer.Serialize(address.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Address>("address", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}