using LarsProjekt.Domain.StoreApi;
using LarsProjekt.Domain.Interfaces;
using LarsProjekt.Dto.StoreApi;
using LarsProjekt.Dto.StoreApi.Mapping;
using System.Text.Json;

namespace LarsProjekt.StoreApiAdapter;

internal class CouponService // : ICouponService
{
    private readonly IStoreApiClient _client;

    public CouponService(IStoreApiClient client)
    {
        _client = client;
    }
    public async Task<List<Coupon>> GetCoupons()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<CouponDto>>("coupon", "getall", HttpMethod.Get);

        var coupons = new List<Coupon>();
        foreach (var coupon in content)
        {
            coupons.Add(coupon.ToDomain());
        }
        return coupons;
    }

    public async Task<Coupon> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<CouponDto>("coupon", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<Coupon> GetByName(string name)
    {
        var content = await _client.HttpResponseMessageAsyncGet<CouponDto>("coupon", $"getbyname?name={name}", HttpMethod.Get);

        return content.ToDomain();
    }

    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("coupon", $"delete?id={id}", HttpMethod.Delete);
    }
    public async Task<Coupon> Update(Coupon coupon) // error concurrency
    {
        var requestContent = JsonSerializer.Serialize(coupon.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Coupon>("coupon", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<Coupon> Create(Coupon coupon)
    {
        var requestContent = JsonSerializer.Serialize(coupon.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Coupon>("coupon", "create", requestContent, HttpMethod.Post);

        return content;

    }

}