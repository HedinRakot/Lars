using LarsProjekt.Domain.Interfaces;
using LarsProjekt.Domain.MyTemsApi;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Dto.MyTemsApi;
using System.Text.Json;

namespace LarsProjekt.UserApiAdapter;

internal class UserService : IUserService
{
    private readonly IUserApiClient _client;

    public UserService(IUserApiClient client)
    {
        _client = client;
    }

    public async Task<User> GetByName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var content = await _client.HttpResponseMessageAsyncGet<User>("customer", $"getbyname?name={name}", HttpMethod.Get);
        return content;
    }
    public async Task<User> GetByNameWithAddress(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        var content = await _client.HttpResponseMessageAsyncGet<User>("customer", $"getbynamewithaddress?name={name}", HttpMethod.Get);
        return content;
    }
    public async Task<List<User>> Get()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<UserDto>>("customer", "getall", HttpMethod.Get);

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
        var content = await _client.HttpResponseMessageAsyncPost<User>("customer", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<User> Create(User user)
    {
        var requestContent = JsonSerializer.Serialize(user.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<User>("customer", "create", requestContent, HttpMethod.Post);

        return content;
    }
    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("customer", $"delete?id={id}", HttpMethod.Delete);        
    }
}