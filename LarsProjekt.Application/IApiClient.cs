
namespace LarsProjekt.Application
{
    public interface IApiClient
    {
        Task<string> GetHttpResponseMessageAsync(string obj, string uriMethod, HttpMethod httpMethod);
    }
}