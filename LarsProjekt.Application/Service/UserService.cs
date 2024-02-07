using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class UserService : IUserService
{
    private readonly IApiClient _client;

    public UserService(IApiClient client)
    {
        _client = client;
    }

    public async Task<User> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var content = await _client.HttpResponseMessageAsyncGet<User>("users", $"getbyname?name={name}", HttpMethod.Get);
        return content;
    }
    public async Task<User> GetByNameWithAddress(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var content = await _client.HttpResponseMessageAsyncGet<User>("users", $"getbynamewithaddress?name={name}", HttpMethod.Get);
        return content;
    }
    public async Task<List<User>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<UserDto>>("users", "getall", HttpMethod.Get);

        var users = new List<User>();
        foreach (var user in content)
        {
            users.Add(user.ToDomain());
        }

        return users;
    }

    public async Task<User> Update(User user)
    {
        var requestContent = JsonSerializer.Serialize(user.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<User>("users", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<User> Create(User user)
    {
        var requestContent = JsonSerializer.Serialize(user.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<User>("users", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("users", $"delete?id={id}", HttpMethod.Delete);        
    }
}