namespace LarsProjekt.Application;

public class ApiClient : IApiClient
{
    public async Task<string> GetHttpResponseMessageAsync(string obj, string uriMethod, HttpMethod httpMethod)
    {
        var uri = $"https://localhost:7182/api/{obj}/{uriMethod}";
        var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
        httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036");

        var httpClient = new HttpClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return content;
        }
        throw new Exception($"An error occurred during the API call: {httpResponseMessage.StatusCode}");
    }
    
}

