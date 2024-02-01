using LarsProjekt.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LarsProjekt.Application;

public class ApiClient : IApiClient
{   
    public async Task<T> GetHttpResponseMessageAsync<T>(string obj, string uriMethod, HttpMethod httpMethod)
        where T : class, new()
    {       
        var uri = $"https://localhost:7182/api/{obj}/{uriMethod}";
        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");

        var httpClient = new HttpClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        throw new Exception($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
    }
    
}

//private readonly ApiUserOptions _userOptions;
//private readonly IConfiguration _configuration;
//public ApiClient(IOptions<ApiUserOptions> userOptions, IConfiguration configuration)
//{
//    _userOptions = userOptions.Value;
//    _configuration = configuration;
//}

//var list = _userOptions.AppUserDtos;
//var userList = new ApiUserOptions();
//_configuration.GetSection(ApiUserOptions.Section).Bind(userList);