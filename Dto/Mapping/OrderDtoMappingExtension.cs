using LarsProjekt.Domain;

namespace LarsProjekt.Dto.Mapping;

public static class OrderDtoMappingExtension
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.Total,
            order.Date,
            order.AddressId,
            order.UserId,
            order.Details
            );
    }
    public static Order ToDomain(this OrderDto dto)
    {
        return new Order
        {
            Id = dto.Id,
            Date = dto.Date,
            Total = dto.Total,
            AddressId = dto.AddressId,
            UserId = dto.UserId
        };
    }
}
