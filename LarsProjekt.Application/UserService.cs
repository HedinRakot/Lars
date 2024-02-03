using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class UserService : IUserService
{
    private readonly IApiClient _client;
    private readonly IAddressService _addressService;

    public UserService(IApiClient client, IAddressService addressService)
    {
        _client = client;
        _addressService = addressService;
    }

    public async Task<User> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var content = await _client.GetHttpResponseMessageAsync<UserDto>("users", $"getbyname?name={name}", HttpMethod.Get);
        var domainContent = content.ToDomain();
        domainContent.Address = await _addressService.GetById(content.AddressId);

        return domainContent;
    }
    public async Task<List<User>> Get()
    {
        var content = await _client.GetHttpResponseMessageAsync<List<UserDto>>("users", "getall", HttpMethod.Get);

        var users = new List<User>();
        foreach (var user in content)
        {
            users.Add(user.ToDomain());
        }

        return users;
    }

    public async Task<User> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<UserDto>("users", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<User> Update(User user)
    {
        var requestContent = JsonSerializer.Serialize(user.ToDto());
        var content = await _client.PostHttpResponseMessageAsync<User>("users", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<User> Create(User user)
    {
        var requestContent = JsonSerializer.Serialize(user.ToDto());
        var content = await _client.PostHttpResponseMessageAsync<User>("users", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<UserDto>("users", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}