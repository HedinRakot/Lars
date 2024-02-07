
namespace LarsProjekt.Application
{
    public interface IApiClient
    {
        Task<T> HttpResponseMessageAsyncGet<T>(string obj, string uriMethod, HttpMethod httpMethod);
        Task<T> HttpResponseMessageAsyncPost<T>(string obj, string uriMethod, string content, HttpMethod httpMethod);
        Task HttpResponseMessageAsyncDelete(string obj, string uriMethod, HttpMethod httpMethod);
    }
}