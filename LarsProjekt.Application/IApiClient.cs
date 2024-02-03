
namespace LarsProjekt.Application
{
    public interface IApiClient
    {
        Task<T> GetHttpResponseMessageAsync<T>(string obj, string uriMethod, HttpMethod httpMethod);
        Task<T> PostHttpResponseMessageAsync<T>(string obj, string uriMethod, string content, HttpMethod httpMethod);
    }
}