using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTemsAPI.Authentication;
using MyTemsAPI.Domain.IRepositories;
using MyTemsAPI.Dto;
using MyTemsAPI.Dto.Mapping;
using MyTemsAPI.Models.Mapping;

namespace MyTemsAPI.Controllers
{       
    public static class AddressEndpoints
    {        
        public static void MapAddressEndpoints(this IEndpointRouteBuilder builder)
        {
            //var group = builder.MapGroup("address");
            builder.MapGet("address", GetAll);
            builder.MapGet("address/getbyid/{id}", GetById);
            builder.MapPost("address/create", Create);
            builder.MapPut("address/update", Put);
            builder.MapDelete("address/delete/{id}", Delete);
        }
        public static async Task<IResult> GetAll(IAddressRepository addressRepository)
        {
            var addresses = await addressRepository.GetAll();
            return Results.Ok(addresses);
        }
        public static async Task<IResult> GetById(long id, IAddressRepository addressRepository)
        {
            var address = await addressRepository.GetById(id);
            return Results.Ok(address);
        }
        public static async Task<IResult> Create(IAddressRepository addressRepository, [FromBody] AddressDto dto)
        {
            var address = dto.ToDomain();
            await addressRepository.Add(address);
            return Results.Ok(address);
        }
        public static async Task<IResult> Put(IAddressRepository addressRepository, [FromBody] AddressDto dto)
        {
            try
            {
                var address = dto.ToDomain();
                await addressRepository.Update(address);
                return Results.Ok(address);
            }
            catch
            {
                return Results.NotFound();
            }
        }
        public static async Task<IResult> Delete(long id, IAddressRepository addressRepository)
        {
            try
            {
                var address = await addressRepository.GetById(id);
                await addressRepository.Delete(address);
                return Results.Ok(address);
            }
            catch
            {
                return Results.NotFound();
            }                        
        }
    }
}
