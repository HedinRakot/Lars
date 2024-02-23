using LarsProjekt.Domain;
using LarsProjekt.Messages.Dtos;

namespace LarsProjekt.Messages.Mapping;

public static class OrderDetailEventDtoMappingExtension
{
    //public static OrderDetailEventDto ToEventDto(this OrderDetail detail)
    //{
    //    return new OrderDetailEventDto(
    //        detail.Id,
    //        detail.OrderId,
    //        detail.ProductId,
    //        detail.Quantity,
    //        detail.UnitPrice,
    //        detail.DiscountedPrice,
    //        detail.Discount
    //        );
    //}
    public static OrderDetail ToDomain(this OrderDetailEventDto dto)
    {
        return new OrderDetail
        {
            Id = dto.Id,
            OrderId = dto.OrderId,
            ProductId = dto.ProductId,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            DiscountedPrice = dto.DiscountedPrice,
            Discount = dto.Discount
        };
    }
}
