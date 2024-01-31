using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application;

internal class CouponService : ICouponService
{
    private readonly IApiClient _client;

    public CouponService(IApiClient client)
    {
        _client = client;
    }
    public async Task<List<Coupon>> GetCoupons()
    {
        var content = await _client.GetHttpResponseMessageAsync("coupons", "getall", HttpMethod.Get);

        var couponDtos = JsonSerializer.Deserialize<List<CouponDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var coupons = new List<Coupon>();
        foreach (var coupon in couponDtos)
        {
            coupons.Add(coupon.ToDomain());
        }

        return coupons;


        throw new Exception();
    }

    //public async Task<Coupon> GetById(long id)
    //{
    //    var httpClient = new HttpClient();
    //    var httpResponseMessage = await httpClient.SendAsync(GetMessage("coupons", $"getbyid?id={id}", HttpMethod.Get));

    //    if (httpResponseMessage.IsSuccessStatusCode)
    //    {
    //        var content = await httpResponseMessage.Content.ReadAsStringAsync();

    //        var productDto = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        });

    //        return productDto.ToDomain();
    //    }

    //    throw new Exception();
    //}
    //public async Task<Product> Update(Product product)
    //{
    //    var httpRequestMessage = GetMessage("update", HttpMethod.Post);
    //    var requestContent = JsonSerializer.Serialize(product.ToDto());
    //    httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

    //    var httpClient = new HttpClient();
    //    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

    //    if (httpResponseMessage.IsSuccessStatusCode)
    //    {
    //        var content = await httpResponseMessage.Content.ReadAsStringAsync();

    //        var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        });

    //        return result.ToDomain();
    //    }

    //    throw new Exception();
    //}
    //public async Task<Product> Create(Product product)
    //{
    //    var httpRequestMessage = GetMessage("create", HttpMethod.Post);
    //    var requestContent = JsonSerializer.Serialize(product.ToDto());
    //    httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

    //    var httpClient = new HttpClient();
    //    var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

    //    if (httpResponseMessage.IsSuccessStatusCode)
    //    {
    //        var content = await httpResponseMessage.Content.ReadAsStringAsync();

    //        var result = JsonSerializer.Deserialize<ProductDto>(content, new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        });

    //        return result.ToDomain();
    //    }

    //    throw new Exception();
    //}
    //public async Task<string> Delete(long id)
    //{
    //    var httpClient = new HttpClient();
    //    var httpResponseMessage = await httpClient.SendAsync(GetMessage($"delete?id={id}", HttpMethod.Delete));

    //    if (httpResponseMessage.IsSuccessStatusCode)
    //    {
    //        return await httpResponseMessage.Content.ReadAsStringAsync();
    //    }
    //    throw new Exception();
    //}
}



//var uri = "https://localhost:7182/api/products/getall";
//var httpClient = new HttpClient();
//var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
//httpRequestMessage.Headers.Add("x-api-key", "A886EA8A-7AB3-4C7D-B248-02989374E036"); // KeyValue Pair, aus config???