using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class UserService : IUserService
{
    private readonly IApiClient _client;

    public UserService(IApiClient client)
    {
        _client = client;
    }

    public async Task<User> GetByName(string name)
    {
        var content = await _client.GetHttpResponseMessageAsync<User>("users", $"getbyname?name={name}", HttpMethod.Get);

        return content;
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
        var content = await _client.GetHttpResponseMessageAsync<User>("users", $"getbyid?id={id}", HttpMethod.Get);

        return content;
    }
    public async Task<User> Update(User user)
    {
        var content = await _client.GetHttpResponseMessageAsync<User>("users", "update", HttpMethod.Put);

        //var requestContent = JsonSerializer.Serialize(user.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<User> Create(User user)
    {
        var content = await _client.GetHttpResponseMessageAsync<User>("users", "create", HttpMethod.Post);

        //var requestContent = JsonSerializer.Serialize(user.ToDto());
        //httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

        return content;
    }
    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<User>("users", $"delete?id={id}", HttpMethod.Delete);
        return content.ToString();
    }
}