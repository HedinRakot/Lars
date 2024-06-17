using LarsProjekt.Domain.Exceptions;
using Microsoft.Extensions.Options;
using System.Text.Json;
using IdentityModel.Client;


namespace LarsProjekt.Application;

public class ApiClient : IApiClient
{
    //private readonly ApiUserOptions _userOptions;
    private readonly ApiUrlOptions _urlOptions;
    public ApiClient(IOptions<ApiUserOptions> userOptions, IOptions<ApiUrlOptions> urlOptions)
    {
        //_userOptions = userOptions.Value;
        _urlOptions = urlOptions.Value;
    }
    public async Task<T> HttpResponseMessageAsyncGet<T>(string obj, string uriMethod, HttpMethod httpMethod)
    {
        try
        {
            var httpClient = await GetClientWithAuth();
            HttpRequestMessage httpRequestMessage = GetRequestMessage(obj, uriMethod, httpMethod);
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var response = await Deserialize<T>(httpResponseMessage);
                return response;
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
            var httpClient = await GetClientWithAuth();
            HttpRequestMessage httpRequestMessage = GetRequestMessage(obj, uriMethod, httpMethod);
            httpRequestMessage.Content = new StringContent(content, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode) 
            { 
                var response = await Deserialize<T>(httpResponseMessage);
                return response;
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
            var httpClient = await GetClientWithAuth();
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
        //var users = _userOptions.Users;
        //var key = users.FirstOrDefault().Key;
        var uri = $"{apiUrl}/{obj}/{uriMethod}";
        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
        //httpRequestMessage.Headers.Add("x-api-key", key);
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

    private static async Task<HttpClient> GetClientWithAuth()
    {
        var tokenResponse = await GetTokenResponse();
        var apiClient = new HttpClient();
        apiClient.SetBearerToken(tokenResponse.AccessToken!);
        return apiClient;
    }
    private static async Task<IdentityModel.Client.TokenResponse> GetTokenResponse()
    {
        // discover endpoints from metadata
        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7099");
        if (disco.IsError)
        {
            Console.WriteLine(disco.Error);
        }

        // request token
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "mytemsapi",
            ClientSecret = "secret",
            Scope = "aspnetmvcscope"
        });

        if (tokenResponse.IsError)
        {
            Console.WriteLine(tokenResponse.Error);
            Console.WriteLine(tokenResponse.ErrorDescription);
        }

        Console.WriteLine(tokenResponse.AccessToken);

        return tokenResponse;
    }
            //var response = await apiClient.GetAsync("https://localhost:7099/identity");
            //    if (!response.IsSuccessStatusCode)
            //    {
            //        Console.WriteLine(response.StatusCode);
            //    }
            //    else
            //    {
            //        var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
            //Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
            //    }

}
