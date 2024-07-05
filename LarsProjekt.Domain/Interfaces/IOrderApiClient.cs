namespace LarsProjekt.Domain.Interfaces
{
    public interface IOrderApiClient
    {
        Task HttpResponseMessageAsyncDelete(string obj, string uriMethod, HttpMethod httpMethod);
        Task<T> HttpResponseMessageAsyncGet<T>(string obj, string uriMethod, HttpMethod httpMethod);
        Task<T> HttpResponseMessageAsyncPost<T>(string obj, string uriMethod, string content, HttpMethod httpMethod);
    }
}