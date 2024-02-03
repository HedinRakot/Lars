using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LarsProjekt.Application;

public class ApiClient : IApiClient
{
    private readonly ApiUserOptions _userOptions;
    public ApiClient(IOptions<ApiUserOptions> userOptions)
    {
        _userOptions = userOptions.Value;
    }
    public async Task<T> GetHttpResponseMessageAsync<T>(string obj, string uriMethod, HttpMethod httpMethod)        
    {
        var users = _userOptions.Users; // null
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

    public async Task<T> PostHttpResponseMessageAsync<T>(string obj, string uriMethod, string content, HttpMethod httpMethod)
    {
        var uri = $"https://localhost:7182/api/{obj}/{uriMethod}";
        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");
        httpRequestMessage.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
        var httpClient = new HttpClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        throw new Exception($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
    }

}