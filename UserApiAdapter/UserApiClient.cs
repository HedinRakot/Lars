using LarsProjekt.Domain.Exceptions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using LarsProjekt.Domain.Interfaces;

namespace LarsProjekt.UserApiAdapter;

public class UserApiClient : IUserApiClient
{
    private readonly ApiUserOptions _userOptions;
    private readonly UserApiUrlOptions _urlOptions;
    public UserApiClient(IOptions<ApiUserOptions> userOptions, IOptions<UserApiUrlOptions> urlOptions)
    {
        _userOptions = userOptions.Value;
        _urlOptions = urlOptions.Value;
    }
    public async Task<T> HttpResponseMessageAsyncGet<T>(string obj, string uriMethod, HttpMethod httpMethod)
    {
        try
        {
            var httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = GetRequestMessage(obj, uriMethod, httpMethod);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await Deserialize<T>(httpResponseMessage);
            }
            throw new DomainException($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred during the API call: {ex}");
        }
    }

    public async Task<T> HttpResponseMessageAsyncPost<T>(string obj, string uriMethod, string content, HttpMethod httpMethod)
    {
        try
        {
            var httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = GetRequestMessage(obj, uriMethod, httpMethod);
            httpRequestMessage.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await Deserialize<T>(httpResponseMessage);
            }
            else
            {
                var resContent = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new DomainException($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred during the API call: {ex}");
        }
    }
    public async Task HttpResponseMessageAsyncDelete(string obj, string uriMethod, HttpMethod httpMethod)
    {
        try
        {
            var httpClient = new HttpClient();
            HttpRequestMessage httpRequestMessage = GetRequestMessage(obj, uriMethod, httpMethod);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new DomainException($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            throw new DomainException($"An unexpected error occurred during the API call: {ex}");
        }
    }

    private HttpRequestMessage GetRequestMessage(string obj, string uriMethod, HttpMethod httpMethod)
    {
        var apiUrl = _urlOptions.ApplicationUrl;
        var users = _userOptions.Users;
        var key = users.FirstOrDefault().Key;
        var uri = $"{apiUrl}/{obj}/{uriMethod}";
        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
        httpRequestMessage.Headers.Add("x-api-key", key);
        return httpRequestMessage;
    }

    private static async Task<T?> Deserialize<T>(HttpResponseMessage msg)
    {
        var responseContent = await msg.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T?>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}

