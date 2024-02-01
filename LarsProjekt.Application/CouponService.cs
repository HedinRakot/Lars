using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;

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
        var content = await _client.GetHttpResponseMessageAsync<List<CouponDto>>("coupons", "getall", HttpMethod.Get);

        var coupons = new List<Coupon>();
        foreach (var coupon in content)
        {
            coupons.Add(coupon.ToDomain());
        }
        return coupons;
    }

    public async Task<Coupon> GetById(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Coupon>("coupons", $"getbyid?id={id}", HttpMethod.Get);  // RECORD GEHT NICHT

        return content;
    }

    public async Task<string> Delete(long id)
    {
        var content = await _client.GetHttpResponseMessageAsync<Coupon>("coupons", $"delete?id={id}", HttpMethod.Delete);

        return content.ToString();

    }

    // Wie in den Post und Put Methoden das object (coupon.ToDto) übergeben?


    //public async Task<Coupon> Update(Coupon coupon)
    //{
    //    var content = await _client.GetHttpResponseMessageAsync<Coupon>("coupons", "update", HttpMethod.Put);

    //    var requestContent = JsonSerializer.Serialize(coupon.ToDto());
    //    httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");


    //    return content;
    //}
    //public async Task<Coupon> Create(Coupon coupon)
    //{
    //    var content = await _client.GetHttpResponseMessageAsync<Coupon>("coupons", "create", HttpMethod.Post);

    //    var requestContent = JsonSerializer.Serialize(coupon.ToDto());
    //    httpRequestMessage.Content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

    //    return content;

    //}

}