
namespace LarsProjekt.Application
{
    public interface IApiClient
    {
        Task<T> GetHttpResponseMessageAsync<T>(string obj, string uriMethod, HttpMethod httpMethod) where T : class, new();
    }
}