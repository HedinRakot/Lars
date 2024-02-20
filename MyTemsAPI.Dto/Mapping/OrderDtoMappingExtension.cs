using MyTemsAPI.Domain;

namespace MyTemsAPI.Dto.Mapping;

public static class OrderDtoMappingExtension
{
    public static Order ToDomain(this OrderDto dto)
    {
        var list = new List<OrderDetail>();
        foreach(var item in dto.Details)
        {
            list.Add(item.ToDomain());
        }

        return new Order
        {
            Id = dto.Id,
            Date = dto.Date,
            Total = dto.Total,
            AddressId = dto.AddressId,
            UserId = dto.UserId,
            Details = list
        };
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
