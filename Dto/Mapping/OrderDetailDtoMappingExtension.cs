using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class OrderDetailDtoMappingExtension
{
    public static OrderDetailDto ToDto(this OrderDetail detail)
    {
        return new OrderDetailDto(
            detail.Id,
            detail.OrderId,
            detail.ProductId,
            detail.Quantity,
            detail.UnitPrice,
            detail.DiscountedPrice,
            detail.Discount
            );
    }
    public static OrderDetail ToDomain(this OrderDetailDto dto)
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
