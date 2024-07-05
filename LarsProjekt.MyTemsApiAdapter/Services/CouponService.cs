using LarsProjekt.Domain.Interfaces;
using LarsProjekt.Domain.MyTemsApi;
using LarsProjekt.Dto.Mapping;
using LarsProjekt.Dto.MyTemsApi;
using System.Text.Json;

namespace LarsProjekt.MyTemsApiAdapter.Services;

internal class CouponService : ICouponService
{
    private readonly IMyTemsApiClient _client;

    public CouponService(IMyTemsApiClient client)
    {
        _client = client;
    }
    public async Task<List<Coupon>> GetCoupons()
    {
        var content = await _client.HttpResponseMessageAsyncGet<List<CouponDto>>("coupons", "getall", HttpMethod.Get);

        var coupons = new List<Coupon>();
        foreach (var coupon in content)
        {
            coupons.Add(coupon.ToDomain());
        }
        return coupons;
    }

    public async Task<Coupon> GetById(long id)
    {
        var content = await _client.HttpResponseMessageAsyncGet<CouponDto>("coupons", $"getbyid?id={id}", HttpMethod.Get);

        return content.ToDomain();
    }
    public async Task<Coupon> GetByName(string name)
    {
        var content = await _client.HttpResponseMessageAsyncGet<CouponDto>("coupons", $"getbyname?name={name}", HttpMethod.Get);

        return content.ToDomain();
    }

    public async Task Delete(long id)
    {
        await _client.HttpResponseMessageAsyncDelete("coupons", $"delete?id={id}", HttpMethod.Delete);
    }
    public async Task<Coupon> Update(Coupon coupon) // error concurrency
    {
        var requestContent = JsonSerializer.Serialize(coupon.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Coupon>("coupons", "update", requestContent, HttpMethod.Put);

        return content;
    }
    public async Task<Coupon> Create(Coupon coupon)
    {
        var requestContent = JsonSerializer.Serialize(coupon.ToDto());
        var content = await _client.HttpResponseMessageAsyncPost<Coupon>("coupons", "create", requestContent, HttpMethod.Post);

        return content;

    }

}