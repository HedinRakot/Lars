using LarsProjekt.Domain.Exceptions;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LarsProjekt.Application;

public class ApiClient : IApiClient
{
    private readonly ApiUserOptions _userOptions;
    private readonly ApiUrlOptions _urlOptions;
    public ApiClient(IOptions<ApiUserOptions> userOptions, IOptions<ApiUrlOptions> urlOptions)
    {
        _userOptions = userOptions.Value;
        _urlOptions = urlOptions.Value;
    }
    public async Task<T> GetHttpResponseMessageAsync<T>(string obj, string uriMethod, HttpMethod httpMethod)        
    {
        try
        {
            var apiUrl = _urlOptions.ApplicationUrl;
            var users = _userOptions.Users;
            var key = users.FirstOrDefault().Key;

            var uri = $"{apiUrl}/{obj}/{uriMethod}";
            var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
            httpRequestMessage.Headers.Add("x-api-key", key);
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
            throw new DomainException($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred during the API call: {ex}");
        }
        
    }

    public async Task<T> PostHttpResponseMessageAsync<T>(string obj, string uriMethod, string content, HttpMethod httpMethod)
    {
        try
        {
            var apiUrl = _urlOptions.ApplicationUrl;
            var users = _userOptions.Users;
            var key = users.FirstOrDefault().Key;

            var uri = $"{apiUrl}/{obj}/{uriMethod}";
            var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
            httpRequestMessage.Headers.Add("x-api-key", key);
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
            throw new DomainException($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred during the API call: {ex}");
        }
    }
}
