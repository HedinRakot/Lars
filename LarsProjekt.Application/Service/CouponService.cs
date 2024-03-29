﻿using LarsProjekt.Application.IService;
using LarsProjekt.Domain;
using LarsProjekt.Dto;
using LarsProjekt.Dto.Mapping;
using System.Text.Json;

namespace LarsProjekt.Application.Service;

internal class CouponService : ICouponService
{
    private readonly IApiClient _client;

    public CouponService(IApiClient client)
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